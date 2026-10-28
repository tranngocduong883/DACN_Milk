import { useState } from 'react'
import type { FormEvent } from 'react'
import { Link, useNavigate, useSearchParams } from 'react-router-dom'
import { useStore } from '../context/StoreContext'
import { CartIcon, HelpIcon, MenuIcon, PhoneIcon, SearchIcon, UserIcon } from './Icons'

const Header = () => {
    const { session, user, cartCount, categories } = useStore()
    const navigate = useNavigate()
    const [params] = useSearchParams()
    const [keyword, setKeyword] = useState(params.get('q') ?? '')
    const [menuOpen, setMenuOpen] = useState(false)

    const onSearch = (e: FormEvent) => {
        e.preventDefault()
        navigate(keyword.trim() ? `/?q=${encodeURIComponent(keyword.trim())}` : '/')
    }

    return (
        <header className="sticky top-0 z-30 bg-white shadow-sm">
            {/* Thanh trên cùng */}
            <div className="bg-navy text-white text-sm">
                <div className="flex justify-end gap-6 px-4 md:px-8 py-1.5">
                    <a href="tel:1800545440" className="flex items-center gap-1.5 hover:underline">
                        <PhoneIcon size={16} /> 1800 545440
                    </a>
                    <a href="#footer" className="flex items-center gap-1.5 hover:underline">
                        <HelpIcon size={16} /> Hỗ trợ
                    </a>
                </div>
            </div>

            {/* Logo, tìm kiếm, tài khoản, giỏ hàng */}
            <div className="flex flex-wrap items-center gap-4 px-4 md:px-12 pt-3">
                <Link to="/" className="shrink-0">
                    <img src="/images/logo.png" alt="NutriMilk" className="h-12 md:h-16" />
                </Link>

                <form onSubmit={onSearch} className="order-last md:order-none w-full md:w-auto md:flex-1 md:mx-10 relative">
                    <input
                        value={keyword}
                        onChange={e => setKeyword(e.target.value)}
                        placeholder="Tìm kiếm sản phẩm..."
                        className="w-full rounded-full border border-gray-300 py-2.5 pl-5 pr-14 outline-none focus:border-sky-dark"
                    />
                    <button
                        type="submit"
                        className="absolute right-1.5 top-1/2 -translate-y-1/2 grid place-items-center size-9 rounded-full bg-sky text-white hover:bg-sky-dark"
                        aria-label="Tìm kiếm"
                    >
                        <SearchIcon size={20} />
                    </button>
                </form>

                <div className="ml-auto flex items-center gap-3 md:gap-7">
                    <Link
                        to={session ? '/profile' : '/login'}
                        className="flex items-center gap-3 rounded-full border border-cyan px-4 py-1.5 hover:bg-sky-50"
                    >
                        {session ? (
                            <span className="grid place-items-center size-9 rounded-full bg-sky-100 text-brand">
                                <UserIcon size={18} />
                            </span>
                        ) : (
                            <UserIcon size={22} />
                        )}
                        <span className="leading-tight text-sm">
                            <span className="block font-semibold">{session ? 'Xin chào' : 'Đăng nhập'}</span>
                            <span className="block text-gray-500">{session ? (user?.fullName || session.username) : 'Tài khoản'}</span>
                        </span>
                    </Link>
                    <Link
                        to="/cart"
                        className="flex items-center gap-2 rounded-full border border-cyan px-4 py-2.5 hover:bg-sky-50"
                    >
                        <CartIcon size={24} className="text-cyan" />
                        <span>Giỏ hàng ({cartCount})</span>
                    </Link>
                </div>
            </div>

            {/* Thanh menu */}
            <nav className="flex items-stretch px-4 md:px-12 mt-2">
                <div className="relative w-56 md:w-[350px]" onMouseLeave={() => setMenuOpen(false)}>
                    <button
                        onClick={() => setMenuOpen(o => !o)}
                        onMouseEnter={() => setMenuOpen(true)}
                        className="flex w-full items-center gap-2 bg-sky px-5 py-4 text-white font-medium uppercase"
                    >
                        <MenuIcon size={22} /> Danh mục
                    </button>
                    {menuOpen && (
                        <ul className="absolute left-0 right-0 top-full bg-white shadow-lg border border-gray-100">
                            {categories.map(c => (
                                <li key={c.id}>
                                    <Link
                                        to={`/?category=${c.id}`}
                                        onClick={() => setMenuOpen(false)}
                                        className="block px-5 py-3 hover:bg-sky-50 hover:text-brand"
                                    >
                                        {c.name}
                                    </Link>
                                </li>
                            ))}
                        </ul>
                    )}
                </div>
                <Link to="/?new=1" className="flex items-center px-6 md:px-14 font-medium uppercase hover:text-brand">
                    Sản phẩm mới
                </Link>
                <Link to="/stores" className="hidden sm:flex items-center px-6 md:px-8 font-medium uppercase hover:text-brand">
                    Danh sách cửa hàng
                </Link>
            </nav>
        </header>
    )
}

export default Header
