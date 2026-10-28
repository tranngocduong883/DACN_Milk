import { useState } from 'react'
import type { FormEvent } from 'react'
import { productApi } from '../../api'
import type { Product, ProductInput } from '../../api'
import { useApi } from '../../hooks/useApi'
import { useStore } from '../../context/StoreContext'
import { errorText } from '../../utils/format'
import { ErrorBox, Field, FormActions, Loading, Modal, Select, TextArea } from '../../components/Ui'

const empty: ProductInput = { maSp: '', tenSp: '', maDanhMuc: '', gia: 0, moTa: '', soLuongTon: 0, theTich: undefined, donVi: '' }

const ProductManager = () => {
    const { categories, notify } = useStore()
    const { data, loading, error, reload } = useApi(() => productApi.all(), [])
    const [form, setForm] = useState<ProductInput | null>(null)
    const [editing, setEditing] = useState<Product | null>(null)
    const [saving, setSaving] = useState(false)
    const [keyword, setKeyword] = useState('')

    const openNew = () => { setForm({ ...empty, maDanhMuc: categories[0]?.id ?? '' }); setEditing(null) }
    const openEdit = (p: Product) => {
        setForm({
            maSp: p.id, tenSp: p.name, maDanhMuc: p.categoryId, gia: p.originalPrice, moTa: p.description,
            soLuongTon: p.stock, theTich: p.volume ?? undefined, donVi: p.unit,
        })
        setEditing(p)
    }

    const onSave = async (e: FormEvent) => {
        e.preventDefault()
        if (!form) return
        if (form.gia <= 0) { notify('Giá bán phải lớn hơn 0', 'error'); return }
        setSaving(true)
        try {
            const body = { ...form, maSp: form.maSp.trim(), tenSp: form.tenSp.trim() }
            notify(await (editing ? productApi.update(body) : productApi.create(body)))
            setForm(null)
            await reload()
        } catch (err) {
            notify(errorText(err), 'error')
        } finally {
            setSaving(false)
        }
    }

    const remove = async (p: Product) => {
        if (!confirm(`Xóa sản phẩm "${p.name}"?`)) return
        try {
            notify(await productApi.remove(p.id))
            await reload()
        } catch (err) {
            notify(errorText(err), 'error')
        }
    }

    if (loading && !data) return <Loading />
    if (error && !data) return <ErrorBox message={error} onRetry={reload} />

    const k = keyword.trim().toLowerCase()
    const list = (data ?? []).filter(p => !k || `${p.id} ${p.name}`.toLowerCase().includes(k))
    const num = (v: string) => (v === '' ? undefined : Number(v))

    return (
        <div>
            <h1 className="mb-6 text-2xl font-bold">Quản lý sản phẩm</h1>
            <div className="mb-5 flex flex-wrap items-center justify-between gap-4">
                <button onClick={openNew} className="rounded-md bg-green-600 px-5 py-2.5 text-lg text-white hover:bg-green-700">Thêm sản phẩm</button>
                <input
                    value={keyword}
                    onChange={e => setKeyword(e.target.value)}
                    placeholder="Tìm theo mã hoặc tên sản phẩm"
                    className="w-72 rounded border border-gray-300 px-3 py-2 outline-none focus:border-sky-dark"
                />
            </div>
            <div className="overflow-x-auto">
                <table className="admin-table text-lg">
                    <thead>
                        <tr><th>STT</th><th>Ảnh</th><th>Mã SP</th><th>Tên sản phẩm</th><th>Danh mục</th><th>Đơn vị</th><th>Giá bán</th><th>Tồn kho</th><th>Sửa</th><th>Xóa</th></tr>
                    </thead>
                    <tbody>
                        {list.map((p, i) => (
                            <tr key={p.id}>
                                <td>{i + 1}</td>
                                <td><img src={p.image} alt="" className="mx-auto size-14 object-contain" /></td>
                                <td>{p.id}</td>
                                <td className="!text-left">{p.name}</td>
                                <td>{p.categoryName}</td>
                                <td>{p.unit}</td>
                                <td>
                                    {p.originalPrice.toLocaleString('vi-VN')}
                                    {p.price < p.originalPrice && <span className="block text-sm text-red-500">KM: {p.price.toLocaleString('vi-VN')}</span>}
                                </td>
                                <td>{p.stock}</td>
                                <td><button onClick={() => openEdit(p)} className="rounded bg-blue-500 px-4 py-1.5 text-white hover:bg-blue-600">Sửa</button></td>
                                <td>
                                    <button onClick={() => remove(p)} className="rounded bg-red-500 px-4 py-1.5 text-white hover:bg-red-600">Xóa</button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>

            {form && (
                <Modal title={editing ? 'Sửa sản phẩm' : 'Thêm sản phẩm'} onClose={() => setForm(null)} wide>
                    <form onSubmit={onSave} className="grid gap-4 sm:grid-cols-2">
                        <Field label="Mã sản phẩm" required disabled={!!editing} value={form.maSp} onChange={e => setForm({ ...form, maSp: e.target.value })} />
                        <Field label="Tên sản phẩm" required value={form.tenSp} onChange={e => setForm({ ...form, tenSp: e.target.value })} />
                        <Select label="Danh mục" required value={form.maDanhMuc} onChange={e => setForm({ ...form, maDanhMuc: e.target.value })}>
                            {categories.map(c => <option key={c.id} value={c.id}>{c.name}</option>)}
                        </Select>
                        <Field label="Giá bán (đ)" type="number" min={0} required value={form.gia} onChange={e => setForm({ ...form, gia: Number(e.target.value) })} />
                        <Field label="Số lượng tồn" type="number" min={0} value={form.soLuongTon ?? ''} onChange={e => setForm({ ...form, soLuongTon: num(e.target.value) })} />
                        <Field label="Thể tích (ml)" type="number" min={0} step="any" value={form.theTich ?? ''} onChange={e => setForm({ ...form, theTich: num(e.target.value) })} />
                        <Field label="Đơn vị" placeholder="Hộp, Lốc, Thùng..." value={form.donVi ?? ''} onChange={e => setForm({ ...form, donVi: e.target.value })} />
                        <Field
                            label={editing ? 'Thêm ảnh (tải lên Cloudinary)' : 'Ảnh sản phẩm (tải lên Cloudinary)'}
                            type="file"
                            accept="image/*"
                            multiple
                            onChange={e => setForm({ ...form, files: Array.from(e.target.files ?? []) })}
                        />
                        {editing && editing.gallery.length > 0 && (
                            <div className="flex gap-2 sm:col-span-2">
                                {editing.gallery.map((src, i) => <img key={i} src={src} alt="" className="size-16 rounded border object-contain" />)}
                            </div>
                        )}
                        <div className="sm:col-span-2">
                            <TextArea label="Mô tả" value={form.moTa ?? ''} onChange={e => setForm({ ...form, moTa: e.target.value })} />
                        </div>
                        <div className="sm:col-span-2"><FormActions onCancel={() => setForm(null)} saving={saving} /></div>
                    </form>
                </Modal>
            )}
        </div>
    )
}

export default ProductManager
