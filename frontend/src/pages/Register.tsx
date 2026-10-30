import { useState } from 'react'
import type { FormEvent } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { authApi } from '../api'
import { useStore } from '../context/StoreContext'
import { errorText } from '../utils/format'
import AuthField from '../components/AuthField'

const Register = () => {
    const navigate = useNavigate()
    const { notify } = useStore()
    const [form, setForm] = useState({ hoTen: '', tenDangNhap: '', matKhau: '', email: '', soDienThoai: '', diaChi: '' })
    const [error, setError] = useState('')
    const [loading, setLoading] = useState(false)

    const set = (key: keyof typeof form) => (e: { target: { value: string } }) => setForm(f => ({ ...f, [key]: e.target.value }))

    const onSubmit = async (e: FormEvent) => {
        e.preventDefault()
        setError('')
        if (form.matKhau.length < 6) { setError('Mật khẩu phải có ít nhất 6 ký tự'); return }
        if (form.soDienThoai && !/^0\d{9}$/.test(form.soDienThoai)) { setError('Số điện thoại không hợp lệ (10 số, bắt đầu bằng 0)'); return }
        setLoading(true)
        try {
            notify(await authApi.register({
                ...form,
                tenDangNhap: form.tenDangNhap.trim(),
                email: form.email || undefined,
                soDienThoai: form.soDienThoai || undefined,
                diaChi: form.diaChi || undefined,
            }))
            navigate('/login')
        } catch (err) {
            setError(errorText(err))
        } finally {
            setLoading(false)
        }
    }

    return (
        <div className="px-4">
            <form onSubmit={onSubmit} className="mx-auto max-w-[480px] space-y-5 bg-white px-8 py-4 pb-8">
                <h1 className="text-center text-3xl font-bold">ĐĂNG KÝ</h1>
                <AuthField label="Họ tên" required placeholder="Nhập họ và tên" value={form.hoTen} onChange={set('hoTen')} />
                <AuthField label="Tên đăng nhập" required placeholder="Nhập tên đăng nhập" value={form.tenDangNhap} onChange={set('tenDangNhap')} />
                <AuthField label="Mật khẩu" required type="password" placeholder="Nhập mật khẩu" value={form.matKhau} onChange={set('matKhau')} />
                <AuthField label="Email" type="email" placeholder="Nhập email" value={form.email} onChange={set('email')} />
                <AuthField label="Số điện thoại" placeholder="Nhập số điện thoại" value={form.soDienThoai} onChange={set('soDienThoai')} />
                <AuthField label="Địa chỉ" placeholder="Nhập địa chỉ" value={form.diaChi} onChange={set('diaChi')} />
                {error && <p className="text-sm text-red-600">{error}</p>}
                <button disabled={loading} className="w-full rounded-full bg-cyan py-3.5 font-bold text-white hover:brightness-95 disabled:opacity-60">ĐĂNG KÝ</button>
                <Link to="/login" className="block w-full rounded-full border border-cyan py-3.5 text-center font-bold text-cyan hover:bg-sky-50">
                    ĐĂNG NHẬP
                </Link>
            </form>
        </div>
    )
}

export default Register
