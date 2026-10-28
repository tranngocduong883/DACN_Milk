import { NavLink, Navigate, Outlet, useNavigate } from 'react-router-dom'
import { useStore } from '../context/StoreContext'

const menu = [
    { to: '/profile', label: 'Thông tin tài khoản', end: true },
    { to: '/profile/orders', label: 'Quản lý đơn hàng' },
    { to: '/profile/address', label: 'Địa chỉ giao hàng' },
    { to: '/profile/password', label: 'Đổi mật khẩu' },
]

const ProfileLayout = () => {
    const { session, user, isAdmin, logout } = useStore()
    const navigate = useNavigate()

    if (!session) return <Navigate to="/login" replace />

    return (
        <div className="grid gap-10 px-4 py-10 md:px-10 lg:grid-cols-[445px_1fr]">
            <aside className="self-start rounded-lg bg-white p-7 shadow-md">
                <div className="border-b border-gray-200 pb-5 text-center">
                    <p className="text-xl uppercase">Xin chào</p>
                    <p className="mt-1.5 text-lg font-medium text-gray-700">{user?.fullName || session.username}</p>
                    <p className="mt-1 text-gray-500">@{session.username}</p>
                </div>
                <ul className="mt-5 space-y-2 text-lg uppercase">
                    {menu.map(m => (
                        <li key={m.to}>
                            <NavLink
                                to={m.to}
                                end={m.end}
                                className={({ isActive }) =>
                                    `block rounded-md border px-5 py-3.5 ${isActive
                                        ? 'border-blue-200 bg-blue-50 text-blue-600'
                                        : 'border-transparent text-gray-600 hover:text-blue-600'}`
                                }
                            >
                                {m.label}
                            </NavLink>
                        </li>
                    ))}
                    <li>
                        <button
                            onClick={() => { logout(); navigate('/') }}
                            className="w-full px-5 py-3.5 text-left uppercase text-red-600 hover:underline"
                        >
                            Đăng xuất
                        </button>
                    </li>
                </ul>
                {isAdmin && (
                    <NavLink to="/admin" className="mt-4 block rounded-md bg-navy px-5 py-3 text-center text-white hover:bg-navy-dark">
                        Trang quản trị
                    </NavLink>
                )}
            </aside>
            <section className="min-w-0">
                <Outlet />
            </section>
        </div>
    )
}

export default ProfileLayout
