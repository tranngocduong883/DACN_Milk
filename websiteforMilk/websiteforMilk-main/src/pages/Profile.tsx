import { useState } from 'react'
import type { FormEvent } from 'react'
import { userApi } from '../api'
import { useStore } from '../context/StoreContext'
import { errorText } from '../utils/format'
import { Field, Loading } from '../components/Ui'

const Profile = () => {
    const { user, setUser, notify } = useStore()
    const [editing, setEditing] = useState(false)
    const [form, setForm] = useState({ hoTen: '', soDienThoai: '', email: '', diaChi: '' })
    const [saving, setSaving] = useState(false)

    if (!user) return <Loading />

    const startEdit = () => {
        setForm({ hoTen: user.fullName, soDienThoai: user.phone, email: user.email, diaChi: user.address })
        setEditing(true)
    }

    const onSave = async (e: FormEvent) => {
        e.preventDefault()
        setSaving(true)
        try {
            setUser(await userApi.updateProfile(user.id, form))
            notify('Cập nhật thông tin thành công')
            setEditing(false)
        } catch (err) {
            notify(errorText(err), 'error')
        } finally {
            setSaving(false)
        }
    }

    const set = (key: keyof typeof form) => (e: { target: { value: string } }) => setForm(f => ({ ...f, [key]: e.target.value }))

    const fields = [
        ['Họ tên', user.fullName || 'Chưa cập nhật'],
        ['Số điện thoại', user.phone || 'Chưa cập nhật'],
        ['Email', user.email || 'Chưa cập nhật'],
        ['Địa chỉ', user.address || 'Chưa cập nhật'],
        ['Tên đăng nhập', user.username],
        ['Vai trò', user.role === 'admin' ? 'Quản trị viên' : 'Khách hàng'],
    ]

    return (
        <div className="rounded-lg bg-white p-10 shadow-md">
            <h1 className="mb-10 text-center text-4xl font-bold">THẺ THÀNH VIÊN</h1>
            {editing ? (
                <form onSubmit={onSave} className="grid gap-5 sm:grid-cols-2">
                    <Field label="Họ tên" required value={form.hoTen} onChange={set('hoTen')} />
                    <Field label="Số điện thoại" value={form.soDienThoai} onChange={set('soDienThoai')} pattern="0\d{9}" title="10 số, bắt đầu bằng 0" />
                    <Field label="Email" type="email" value={form.email} onChange={set('email')} />
                    <Field label="Địa chỉ" value={form.diaChi} onChange={set('diaChi')} />
                    <div className="flex gap-3 sm:col-span-2">
                        <button disabled={saving} className="rounded-lg bg-blue-600 px-8 py-3 text-lg font-semibold uppercase text-white hover:bg-blue-700 disabled:opacity-60">
                            Lưu
                        </button>
                        <button type="button" onClick={() => setEditing(false)} className="rounded-lg border border-gray-300 px-8 py-3 text-lg uppercase hover:bg-gray-100">
                            Hủy
                        </button>
                    </div>
                </form>
            ) : (
                <>
                    <div className="grid gap-x-5 sm:grid-cols-2 lg:grid-cols-3">
                        {fields.map(([label, value]) => (
                            <div key={label} className="border-b border-gray-200 py-5">
                                <p className="uppercase text-gray-700">{label}</p>
                                <p className="mt-1.5 text-lg">{value}</p>
                            </div>
                        ))}
                    </div>
                    <button onClick={startEdit} className="mt-10 rounded-lg bg-blue-600 px-8 py-3 text-lg font-semibold uppercase text-white hover:bg-blue-700">
                        Sửa thông tin
                    </button>
                </>
            )}
        </div>
    )
}

export default Profile
