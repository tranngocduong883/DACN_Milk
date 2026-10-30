// Lớp gọi API tới backend ASP.NET Core. Mọi request đi qua /api (Vite proxy sang backend).

const BASE = import.meta.env.VITE_API_URL ?? ''
const AUTH_KEY = 'nm_auth'

export interface AuthSession {
    token: string
    userId: string
    username: string
    email?: string | null
    role: string
}

export const getSession = (): AuthSession | null => {
    try {
        const raw = localStorage.getItem(AUTH_KEY)
        return raw ? (JSON.parse(raw) as AuthSession) : null
    } catch {
        return null
    }
}

export const setSession = (s: AuthSession | null) => {
    try {
        if (s) localStorage.setItem(AUTH_KEY, JSON.stringify(s))
        else localStorage.removeItem(AUTH_KEY)
    } catch {
        /* bỏ qua khi trình duyệt chặn localStorage */
    }
}

export class ApiError extends Error {
    status: number
    constructor(message: string, status: number) {
        super(message)
        this.status = status
    }
}

// Được StoreContext gán để tự đăng xuất khi token hết hạn
let onUnauthorized: (() => void) | null = null
export const setUnauthorizedHandler = (fn: (() => void) | null) => { onUnauthorized = fn }

// Backend trả về APIResponse { success, message, data } hoặc chuỗi thuần hoặc ProblemDetails
const extractMessage = (body: unknown, fallback: string): string => {
    if (typeof body === 'string' && body.trim()) return body
    if (body && typeof body === 'object') {
        const b = body as Record<string, unknown>
        if (typeof b.message === 'string' && b.message) return b.message
        if (b.errors && typeof b.errors === 'object') {
            const first = Object.values(b.errors as Record<string, string[]>)[0]
            if (Array.isArray(first) && first[0]) return first[0]
        }
        if (typeof b.title === 'string') return b.title
    }
    return fallback
}

type Method = 'GET' | 'POST' | 'PUT' | 'DELETE'

export async function request<T>(method: Method, path: string, body?: unknown): Promise<T> {
    const headers: Record<string, string> = {}
    const session = getSession()
    if (session?.token) headers.Authorization = `Bearer ${session.token}`

    let payload: BodyInit | undefined
    if (body instanceof FormData) payload = body
    else if (body !== undefined) {
        headers['Content-Type'] = 'application/json'
        payload = JSON.stringify(body)
    }

    let res: Response
    try {
        res = await fetch(BASE + path, { method, headers, body: payload })
    } catch {
        throw new ApiError('Không kết nối được tới máy chủ. Hãy kiểm tra backend đã chạy chưa.', 0)
    }

    const text = await res.text()
    let parsed: unknown = text
    try {
        parsed = text ? JSON.parse(text) : null
    } catch {
        /* phản hồi dạng chuỗi */
    }

    if (res.status === 401) {
        if (session) onUnauthorized?.()
        throw new ApiError('Phiên đăng nhập đã hết hạn, vui lòng đăng nhập lại', 401)
    }
    if (res.status === 403) throw new ApiError('Bạn không có quyền thực hiện thao tác này', 403)
    if (!res.ok) throw new ApiError(extractMessage(parsed, `Lỗi máy chủ (${res.status})`), res.status)
    if (parsed && typeof parsed === 'object' && (parsed as { success?: boolean }).success === false) {
        throw new ApiError(extractMessage(parsed, 'Thao tác thất bại'), res.status)
    }
    return parsed as T
}

// Lấy phần data trong APIResponse
export async function getData<T>(method: Method, path: string, body?: unknown): Promise<T> {
    const res = await request<{ data: T }>(method, path, body)
    return res.data
}

// Lấy thông báo (message) của phản hồi
export async function getMessage(method: Method, path: string, body?: unknown): Promise<string> {
    const res = await request<unknown>(method, path, body)
    return extractMessage(res, 'Thành công')
}
