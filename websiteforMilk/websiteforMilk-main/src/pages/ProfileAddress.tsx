import { useState } from 'react'
import type { FormEvent } from 'react'
import { userApi } from '../api'
import { useStore } from '../context/StoreContext'
import { errorText } from '../utils/format'
import { Loading, TextArea } from '../components/Ui'

const ProfileAddress = () => {
    const { user, setUser, notify } = useStore()
    const [address, setAddress] = useState<string | null>(null)
    const [saving, setSaving] = useState(false)

    if (!user) return <Loading />
    const value = address ?? user.address

    const onSave = async (e: FormEvent) => {
        e.preventDefault()
        setSaving(true)
        try {
            setUser(await userApi.updateProfile(user.id, { diaChi: value.trim() }))
            notify('Đã cập nhật địa chỉ giao hàng')
        } catch (err) {
            notify(errorText(err), 'error')
        } finally {
            setSaving(false)
        }
    }

    return (
        <form onSubmit={onSave} className="space-y-6 rounded-lg bg-white p-10 shadow-md">
            <h1 className="text-center text-3xl font-bold uppercase">Địa chỉ giao hàng</h1>
            <p className="text-gray-600">Địa chỉ mặc định sẽ được điền sẵn khi bạn thanh toán.</p>
            <TextArea label="Địa chỉ" value={value} onChange={e => setAddress(e.target.value)} required />
            <button disabled={saving} className="rounded-lg bg-blue-600 px-8 py-3 text-lg font-semibold uppercase text-white hover:bg-blue-700 disabled:opacity-60">
                Lưu địa chỉ
            </button>
        </form>
    )
}

export default ProfileAddress
