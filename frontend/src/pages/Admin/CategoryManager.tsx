import { useState } from 'react'
import type { FormEvent } from 'react'
import { categoryApi } from '../../api'
import type { Category, CategoryInput } from '../../api'
import { useStore } from '../../context/StoreContext'
import { errorText } from '../../utils/format'
import { Field, FormActions, Modal, TextArea } from '../../components/Ui'

const empty: CategoryInput = { maDanhMuc: '', tenDanhMuc: '', moTa: '' }

const CategoryManager = () => {
    const { categories, reloadCategories, notify } = useStore()
    const [form, setForm] = useState<CategoryInput | null>(null)
    const [editing, setEditing] = useState(false)
    const [saving, setSaving] = useState(false)

    const openNew = () => { setForm(empty); setEditing(false) }
    const openEdit = (c: Category) => { setForm({ maDanhMuc: c.id, tenDanhMuc: c.name, moTa: c.description }); setEditing(true) }

    const onSave = async (e: FormEvent) => {
        e.preventDefault()
        if (!form) return
        setSaving(true)
        try {
            const body = { ...form, maDanhMuc: form.maDanhMuc.trim(), tenDanhMuc: form.tenDanhMuc.trim() }
            notify(await (editing ? categoryApi.update(body) : categoryApi.create(body)))
            setForm(null)
            await reloadCategories()
        } catch (err) {
            notify(errorText(err), 'error')
        } finally {
            setSaving(false)
        }
    }

    const remove = async (c: Category) => {
        if (!confirm(`Xóa danh mục "${c.name}"?`)) return
        try {
            notify(await categoryApi.remove(c.id))
            await reloadCategories()
        } catch (err) {
            notify(errorText(err), 'error')
        }
    }

    return (
        <div>
            <div className="mb-6 flex items-center justify-between">
                <h1 className="text-3xl font-bold">Quản lý danh mục</h1>
                <button onClick={openNew} className="rounded-md bg-blue-600 px-5 py-2.5 text-white hover:bg-blue-700">+ Thêm danh mục</button>
            </div>
            <div className="overflow-x-auto">
                <table className="admin-table text-lg">
                    <thead>
                        <tr><th>STT</th><th>MÃ DANH MỤC</th><th>TÊN DANH MỤC</th><th>MÔ TẢ</th><th>HÀNH ĐỘNG</th></tr>
                    </thead>
                    <tbody>
                        {categories.map((c, i) => (
                            <tr key={c.id}>
                                <td>{i + 1}</td>
                                <td>{c.id}</td>
                                <td>{c.name}</td>
                                <td>{c.description}</td>
                                <td className="whitespace-nowrap">
                                    <button onClick={() => openEdit(c)} className="mr-2 rounded border border-gray-300 px-2 py-0.5 text-base hover:bg-gray-100">Sửa</button>
                                    <button
                                        onClick={() => remove(c)}
                                        className="rounded border border-red-400 px-2 py-0.5 text-base text-red-500 hover:bg-red-50"
                                    >
                                        Xóa
                                    </button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            </div>

            {form && (
                <Modal title={editing ? 'Sửa danh mục' : 'Thêm danh mục'} onClose={() => setForm(null)}>
                    <form onSubmit={onSave} className="space-y-4">
                        <Field label="Mã danh mục" required disabled={editing} value={form.maDanhMuc} onChange={e => setForm({ ...form, maDanhMuc: e.target.value })} />
                        <Field label="Tên danh mục" required value={form.tenDanhMuc} onChange={e => setForm({ ...form, tenDanhMuc: e.target.value })} />
                        <TextArea label="Mô tả" value={form.moTa ?? ''} onChange={e => setForm({ ...form, moTa: e.target.value })} />
                        <FormActions onCancel={() => setForm(null)} saving={saving} />
                    </form>
                </Modal>
            )}
        </div>
    )
}

export default CategoryManager
