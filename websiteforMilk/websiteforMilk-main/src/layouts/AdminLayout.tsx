import { useState } from 'react'
import { Link, Navigate, NavLink, Outlet, useNavigate } from 'react-router-dom'
import { useStore } from '../context/StoreContext'
import { BoxIcon, DashboardIcon, GiftIcon, ListIcon, LogoutIcon, MenuIcon, TagIcon, UsersIcon } from '../components/Icons'

const menu = [
    { to: '/admin', label: 'Dashboard', icon: DashboardIcon, end: true },
    { to: '/admin/users', label: 'Quản lý người dùng', icon: UsersIcon },
    { to: '/admin/products', label: 'Quản lý sản phẩm', icon: BoxIcon },
    { to: '/admin/orders', label: 'Quản lý đơn hàng', icon: ListIcon },
    { to: '/admin/categories', label: 'Quản lý danh mục', icon: TagIcon },
    { to: '/admin/vouchers', label: 'Quản lý voucher', icon: GiftIcon },
    { to: '/admin/reviews', label: 'Quản lý đánh giá', icon: ListIcon },
]

const AdminLayout = () => {
    const { session, user, isAdmin, logout } = useStore()
    const navigate = useNavigate()
    const [collapsed, setCollapsed] = useState(false)

    if (!session) return <Navigate to="/login" replace />
    if (!isAdmin) return <Navigate to="/" replace />

    const displayName = user?.fullName || session.username

    return (
        <div className="flex min-h-screen bg-gray-100">
            <aside
                className={`sticky top-0 flex h-screen shrink-0 flex-col bg-admin text-white transition-all ${collapsed ? 'w-20' : 'w-64 md:w-80'}`}
            >
                <div className="flex items-center justify-between border-b border-white/10 px-5 py-6">
                    {!collapsed && <Link to="/admin" className="text-xl font-bold">Admin Panel</Link>}
                    <button
                        onClick={() => setCollapsed(c => !c)}
                        className="grid place-items-center rounded-md bg-gray-600/60 p-1.5 hover:bg-gray-500"
                        aria-label="Thu gọn menu"
                    >
                        <MenuIcon size={20} />
                    </button>
                </div>

                <nav className="flex-1 space-y-2 overflow-y-auto px-4 py-4">
                    {menu.map(({ to, label, icon: Icon, end }) => (
                        <NavLink
                            key={to}
                            to={to}
                            end={end}
                            title={label}
                            className={({ isActive }) =>
                                `flex items-center gap-4 rounded-lg px-4 py-3 text-lg ${isActive ? 'bg-blue-600' : 'hover:bg-white/10'}`
                            }
                        >
                            <Icon size={22} className="shrink-0" />
                            {!collapsed && <span>{label}</span>}
                        </NavLink>
                    ))}
                </nav>

                <div className="space-y-4 border-t border-white/10 px-4 py-5">
                    <div className="flex items-center gap-3">
                        <span className="grid place-items-center size-10 shrink-0 rounded-full bg-green-500 text-lg font-bold">{displayName.charAt(0).toUpperCase()}</span>
                        {!collapsed && (
                            <div className="leading-tight">
                                <p className="font-semibold">{displayName}</p>
                                <p className="text-sm text-gray-400">{user?.email || session.email}</p>
                            </div>
                        )}
                    </div>
                    <button
                        onClick={() => { logout(); navigate('/login') }}
                        className="flex items-center gap-3 hover:text-gray-300"
                    >
                        <LogoutIcon size={20} /> {!collapsed && 'Đăng xuất'}
                    </button>
                    {!collapsed && (
                        <Link to="/" className="block rounded-lg border border-white/30 px-4 py-3 hover:bg-white/10">
                            Về trang người dùng
                        </Link>
                    )}
                </div>
            </aside>

            <main className="min-w-0 flex-1 p-4 md:p-8">
                <div className="min-h-full rounded-md bg-gray-50 p-6 md:p-8">
                    <Outlet />
                </div>
            </main>
        </div>
    )
}

export default AdminLayout
