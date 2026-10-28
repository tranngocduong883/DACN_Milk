import { useState } from 'react'
import type { FormEvent } from 'react'
import { voucherApi } from '../../api'
import type { Voucher, VoucherInput } from '../../api'
import { useApi } from '../../hooks/useApi'
import { useStore } from '../../context/StoreContext'
import { errorText, formatDate, formatPriceShort, toDateInput } from '../../utils/format'
import { ErrorBox, Field, FormActions, Loading, Modal, TextArea } from '../../components/Ui'

const today = () => new Date().toISOString().slice(0, 10)
const empty = (): VoucherInput => ({ maVoucher: '', tenVoucher: '', giamGia: 0, dieuKienApDung: 0, ngayBatDau: today(), ngayKetThuc: today(), moTa: '' })

const VoucherManager = () => {
    const { notify } = useStore()
    const { data, loading, error, reload } = useApi(() => voucherApi.all(), [])
    const [form, setForm] = useState<VoucherInput | null>(null)
    const [editing, setEditing] = useState(false)
    const [saving, setSaving] = useState(false)

    const openNew = () => { setForm(empty()); setEditing(false) }
    const openEdit = (v: Voucher) => {
        setForm({
            maVoucher: v.code, tenVoucher: v.name, giamGia: v.discount, dieuKienApDung: v.minOrder,
            ngayBatDau: toDateInput(v.startDate), ngayKetThuc: toDateInput(v.endDate), moTa: v.description,
        })
        setEditing(true)
    }

    const onSave = async (e: FormEvent) => {
        e.preventDefault()
        if (!form) return
        if (form.giamGia <= 0) { notify('Giá trị giảm phải lớn hơn 0', 'error'); return }
        setSaving(true)
        try {
            const body: VoucherInput = {
                ...form,
                maVoucher: form.maVoucher.trim().toUpperCase(),
                ngayBatDau: `${form.ngayBatDau}T00:00:00`,
                ngayKetThuc: `${form.ngayKetThuc}T23:59:59`,
            }
            notify(await (editing ? voucherApi.update(body) : voucherApi.create(body)))
            setForm(null)
            await reload()
        } catch (err) {
            notify(errorText(err), 'error')
        } finally {
            setSaving(false)
        }
    }

    const remove = async (v: Voucher) => {
        if (!confirm(`Xóa voucher ${v.code}?`)) return
        try {
            notify(await voucherApi.remove(v.code))
            await reload()
        } catch (err) {
            notify(errorText(err), 'error')
        }
    }

    if (loading && !data) return <Loading />
    if (error && !data) return <ErrorBox message={error} onRetry={reload} />

    return (
        <div>
            <h1 className="mb-6 text-3xl font-bold">Quản lý Voucher</h1>
            <button onClick={openNew} className="mb-5 rounded-md bg-green-500 px-5 py-2.5 text-lg text-white hover:bg-green-600">Thêm Voucher</button>
            <div className="overflow-x-auto">
                <table className="admin-table text-lg [&_td]:text-left [&_td:first-child]:text-center">
                    <thead>
                        <tr><th>STT</th><th>Mã voucher</th><th>Tên voucher</th><th>Giảm giá</th><th>Điều kiện</th><th>Ngày bắt đầu</th><th>Ngày kết thúc</th><th>Trạng thái</th><th>Hành động</th></tr>
                    </thead>
                    <tbody>
                        {(data ?? []).map((v, i) => (
                            <tr key={v.code}>
                                <td>{i + 1}</td>
                                <td>{v.code}</td>
                                <td>{v.name}</td>
                                <td>{formatPriceShort(v.discount)}</td>
                                <td>{formatPriceShort(v.minOrder)}</td>
                                <td>{formatDate(v.startDate)}</td>
                                <td>{formatDate(v.endDate)}</td>
                                <td className="text-base">{v.status}</td>
                                <td className="whitespace-nowrap !text-center">
                                    <button onClick={() => openEdit(v)} className="mr-2 rounded bg-blue-500 px-2.5 py-1 text-white hover:bg-blue-600">Sửa</button>
                                    <button onClick={() => remove(v)} className="rounded bg-red-500 px-2.5 py-1 text-white hover:bg-red-600">Xóa</button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>

            {form && (
                <Modal title={editing ? 'Sửa voucher' : 'Thêm voucher'} onClose={() => setForm(null)}>
                    <form onSubmit={onSave} className="space-y-4">
                        <Field label="Mã voucher" required disabled={editing} value={form.maVoucher} onChange={e => setForm({ ...form, maVoucher: e.target.value })} />
                        <Field label="Tên voucher" required value={form.tenVoucher} onChange={e => setForm({ ...form, tenVoucher: e.target.value })} />
                        <div className="grid gap-4 sm:grid-cols-2">
                            <Field label="Giảm giá (đ)" type="number" min={0} required value={form.giamGia} onChange={e => setForm({ ...form, giamGia: Number(e.target.value) })} />
                            <Field label="Đơn tối thiểu (đ)" type="number" min={0} value={form.dieuKienApDung} onChange={e => setForm({ ...form, dieuKienApDung: Number(e.target.value) })} />
                            <Field label="Ngày bắt đầu" type="date" required value={form.ngayBatDau} onChange={e => setForm({ ...form, ngayBatDau: e.target.value })} />
                            <Field label="Ngày kết thúc" type="date" required value={form.ngayKetThuc} onChange={e => setForm({ ...form, ngayKetThuc: e.target.value })} />
                        </div>
                        <TextArea label="Mô tả" value={form.moTa ?? ''} onChange={e => setForm({ ...form, moTa: e.target.value })} />
                        <FormActions onCancel={() => setForm(null)} saving={saving} />
                    </form>
                </Modal>
            )}
        </div>
    )
}

export default VoucherManager
