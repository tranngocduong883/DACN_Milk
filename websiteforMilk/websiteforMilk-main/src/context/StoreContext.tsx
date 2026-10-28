import { createContext, useCallback, useContext, useEffect, useState } from 'react'
import type { ReactNode } from 'react'
import { authApi, cartApi, categoryApi, userApi } from '../api'
import type { CartItem, Category, User } from '../api'
import { getSession, setSession, setUnauthorizedHandler } from '../api/client'
import type { AuthSession } from '../api/client'
import { errorText } from '../utils/format'

export type { CartItem }

type ToastType = 'success' | 'error'
interface Toast { id: number; text: string; type: ToastType }

interface StoreValue {
    session: AuthSession | null
    user: User | null
    isAdmin: boolean
    login: (username: string, password: string) => Promise<AuthSession>
    logout: () => void
    refreshUser: () => Promise<void>
    setUser: (u: User) => void
    categories: Category[]
    reloadCategories: () => Promise<void>
    cart: CartItem[]
    cartCount: number
    cartTotal: number
    reloadCart: () => Promise<void>
    // Trả về false nếu chưa đăng nhập
    addToCart: (productId: string, quantity?: number) => Promise<boolean>
    updateQuantity: (productId: string, quantity: number) => Promise<void>
    removeFromCart: (productId: string) => Promise<void>
    clearCart: () => Promise<void>
    notify: (text: string, type?: ToastType) => void
}

const StoreContext = createContext<StoreValue | null>(null)

export const StoreProvider = ({ children }: { children: ReactNode }) => {
    const [session, setSessionState] = useState<AuthSession | null>(() => getSession())
    const [user, setUser] = useState<User | null>(null)
    const [categories, setCategories] = useState<Category[]>([])
    const [cart, setCart] = useState<CartItem[]>([])
    const [toasts, setToasts] = useState<Toast[]>([])

    const notify = useCallback((text: string, type: ToastType = 'success') => {
        const id = Date.now() + Math.random()
        setToasts(t => [...t, { id, text, type }])
        setTimeout(() => setToasts(t => t.filter(x => x.id !== id)), 3500)
    }, [])

    const logout = useCallback(() => {
        setSession(null)
        setSessionState(null)
        setUser(null)
        setCart([])
    }, [])

    useEffect(() => {
        setUnauthorizedHandler(() => {
            logout()
            notify('Phiên đăng nhập đã hết hạn, vui lòng đăng nhập lại', 'error')
        })
        return () => setUnauthorizedHandler(null)
    }, [logout, notify])

    const userId = session?.userId

    const refreshUser = useCallback(async () => {
        if (!userId) return
        try {
            setUser(await userApi.get(userId))
        } catch {
            /* lỗi 401 đã được xử lý ở client */
        }
    }, [userId])

    const reloadCart = useCallback(async () => {
        if (!userId) { setCart([]); return }
        try {
            setCart(await cartApi.get(userId))
        } catch {
            // Tài khoản mới chưa có giỏ hàng -> backend trả 404
            setCart([])
        }
    }, [userId])

    const reloadCategories = useCallback(async () => {
        try {
            setCategories(await categoryApi.all())
        } catch (e) {
            notify(errorText(e), 'error')
        }
    }, [notify])

    useEffect(() => { reloadCategories() }, [reloadCategories])
    useEffect(() => { refreshUser(); reloadCart() }, [refreshUser, reloadCart])

    const run = async (fn: () => Promise<CartItem[]>) => {
        try {
            setCart(await fn())
        } catch (e) {
            notify(errorText(e), 'error')
            throw e
        }
    }

    const value: StoreValue = {
        session,
        user,
        isAdmin: session?.role === 'admin',
        login: async (username, password) => {
            const res = await authApi.login(username, password)
            const s: AuthSession = {
                token: res.token,
                userId: res.maTaiKhoan,
                username: res.tenDangNhap,
                email: res.email,
                role: (res.vaiTro ?? 'user').toLowerCase(),
            }
            setSession(s)
            setSessionState(s)
            return s
        },
        logout,
        refreshUser,
        setUser,
        categories,
        reloadCategories,
        cart,
        cartCount: cart.reduce((s, c) => s + c.quantity, 0),
        cartTotal: cart.reduce((s, c) => s + c.quantity * c.product.price, 0),
        reloadCart,
        addToCart: async (productId, quantity = 1) => {
            if (!userId) return false
            await run(() => cartApi.add(userId, productId, quantity))
            notify('Đã thêm sản phẩm vào giỏ hàng')
            return true
        },
        updateQuantity: async (productId, quantity) => {
            if (!userId || quantity < 1) return
            await run(() => cartApi.setQuantity(userId, productId, quantity))
        },
        removeFromCart: async (productId) => {
            if (!userId) return
            await run(() => cartApi.remove(userId, productId))
        },
        clearCart: async () => {
            if (!userId) return
            await run(() => cartApi.clear(userId))
        },
        notify,
    }

    return (
        <StoreContext.Provider value={value}>
            {children}
            <div className="pointer-events-none fixed right-4 top-4 z-[100] flex max-w-sm flex-col gap-2">
                {toasts.map(t => (
                    <div
                        key={t.id}
                        role="status"
                        className={`pointer-events-auto rounded-lg px-4 py-3 text-white shadow-lg ${t.type === 'error' ? 'bg-red-600' : 'bg-green-600'}`}
                    >
                        {t.text}
                    </div>
                ))}
            </div>
        </StoreContext.Provider>
    )
}

// eslint-disable-next-line react-refresh/only-export-components
export const useStore = () => {
    const ctx = useContext(StoreContext)
    if (!ctx) throw new Error('useStore must be used inside StoreProvider')
    return ctx
}
