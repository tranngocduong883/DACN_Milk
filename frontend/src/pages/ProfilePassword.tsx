import { useState } from 'react'
import type { FormEvent } from 'react'
import { userApi } from '../api'
import { useStore } from '../context/StoreContext'
import { errorText } from '../utils/format'
import { Field } from '../components/Ui'

const ProfilePassword = () => {
    const { session, notify } = useStore()
    const [form, setForm] = useState({ old: '', next: '', confirm: '' })
    const [error, setError] = useState('')
    const [saving, setSaving] = useState(false)

    const set = (key: keyof typeof form) => (e: { target: { value: string } }) => setForm(f => ({ ...f, [key]: e.target.value }))

    const onSave = async (e: FormEvent) => {
        e.preventDefault()
        setError('')
        if (form.next.length < 6) { setError('Mật khẩu mới phải có ít nhất 6 ký tự'); return }
        if (form.next !== form.confirm) { setError('Mật khẩu xác nhận không khớp'); return }
        if (!session) return
        setSaving(true)
        try {
            notify(await userApi.changePassword(session.userId, form.old, form.next))
            setForm({ old: '', next: '', confirm: '' })
        } catch (err) {
            setError(errorText(err))
        } finally {
            setSaving(false)
        }
    }

    return (
        <form onSubmit={onSave} className="max-w-xl space-y-5 rounded-lg bg-white p-10 shadow-md">
            <h1 className="text-3xl font-bold uppercase">Đổi mật khẩu</h1>
            <Field label="Mật khẩu hiện tại" type="password" required value={form.old} onChange={set('old')} />
            <Field label="Mật khẩu mới" type="password" required value={form.next} onChange={set('next')} />
            <Field label="Nhập lại mật khẩu mới" type="password" required value={form.confirm} onChange={set('confirm')} />
            {error && <p className="text-red-600">{error}</p>}
            <button disabled={saving} className="rounded-lg bg-blue-600 px-8 py-3 text-lg font-semibold uppercase text-white hover:bg-blue-700 disabled:opacity-60">
                Đổi mật khẩu
            </button>
        </form>
    )
}

export default ProfilePassword
