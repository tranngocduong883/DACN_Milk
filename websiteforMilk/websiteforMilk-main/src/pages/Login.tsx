import { useState } from 'react'
import type { FormEvent } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { useStore } from '../context/StoreContext'
import { errorText } from '../utils/format'
import AuthField from '../components/AuthField'

const Login = () => {
    const { login, notify } = useStore()
    const navigate = useNavigate()
    const [username, setUsername] = useState('')
    const [password, setPassword] = useState('')
    const [error, setError] = useState('')
    const [loading, setLoading] = useState(false)

    const onSubmit = async (e: FormEvent) => {
        e.preventDefault()
        setError('')
        setLoading(true)
        try {
            const s = await login(username.trim(), password)
            notify('Đăng nhập thành công')
            navigate(s.role === 'admin' ? '/admin' : '/')
        } catch (err) {
            setError(errorText(err))
        } finally {
            setLoading(false)
        }
    }

    return (
        <div className="px-4 py-3">
            <form onSubmit={onSubmit} className="mx-auto max-w-[650px] space-y-6 rounded-xl bg-white px-6 py-10 shadow-md md:px-11">
                <h1 className="text-center text-4xl font-bold">ĐĂNG NHẬP</h1>
                <AuthField label="Tên đăng nhập" required placeholder="Nhập tên đăng nhập" value={username} onChange={e => setUsername(e.target.value)} />
                <AuthField label="Mật khẩu" required type="password" placeholder="Nhập mật khẩu" value={password} onChange={e => setPassword(e.target.value)} />
                {error && <p className="text-sm text-red-600">{error}</p>}
                <button disabled={loading} className="w-full rounded-full bg-cyan py-4 text-lg font-bold text-white hover:brightness-95 disabled:opacity-60">
                    {loading ? 'ĐANG ĐĂNG NHẬP...' : 'ĐĂNG NHẬP'}
                </button>
                <Link to="/register" className="block w-full rounded-full border border-cyan py-4 text-center text-lg font-bold text-cyan hover:bg-sky-50">
                    ĐĂNG KÝ
                </Link>
            </form>
        </div>
    )
}

export default Login
