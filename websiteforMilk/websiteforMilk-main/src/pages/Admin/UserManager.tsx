import { useState } from 'react'
import { userApi } from '../../api'
import type { User } from '../../api'
import { useApi } from '../../hooks/useApi'
import { useStore } from '../../context/StoreContext'
import { errorText } from '../../utils/format'
import { ErrorBox, Loading } from '../../components/Ui'

const UserManager = () => {
    const { session, notify } = useStore()
    const { data, loading, error, reload } = useApi(() => userApi.all(), [])
    const [keyword, setKeyword] = useState('')

    const act = async (fn: () => Promise<string>) => {
        try {
            notify(await fn())
            await reload()
        } catch (e) {
            notify(errorText(e), 'error')
        }
    }

    const toggleRole = (u: User) => {
        const next = u.role === 'admin' ? 'user' : 'admin'
        if (confirm(`Đổi vai trò của ${u.username} thành ${next}?`)) act(() => userApi.updateRole(u.id, next))
    }

    const remove = (u: User) => {
        if (confirm(`Xóa tài khoản ${u.username}?`)) act(() => userApi.remove(u.id))
    }

    if (loading && !data) return <Loading />
    if (error && !data) return <ErrorBox message={error} onRetry={reload} />

    const k = keyword.trim().toLowerCase()
    const list = (data ?? []).filter(u => !k || `${u.username} ${u.fullName} ${u.phone} ${u.email}`.toLowerCase().includes(k))

    return (
        <div>
            <div className="mb-6 flex flex-wrap items-center justify-between gap-4">
                <h1 className="text-3xl font-bold">Quản lý người dùng</h1>
                <input
                    value={keyword}
                    onChange={e => setKeyword(e.target.value)}
                    placeholder="Tìm theo tên, SĐT, email..."
                    className="w-72 rounded border border-gray-300 px-3 py-2 outline-none focus:border-sky-dark"
                />
            </div>
            <div className="overflow-x-auto">
                <table className="admin-table text-lg">
                    <thead>
                        <tr><th>STT</th><th>TÊN ĐĂNG NHẬP</th><th>HỌ TÊN</th><th>SĐT</th><th>EMAIL</th><th>VAI TRÒ</th><th>HÀNH ĐỘNG</th></tr>
                    </thead>
                    <tbody>
                        {list.map((u, i) => {
                            const isAdmin = u.role === 'admin'
                            const isMe = u.id === session?.userId
                            return (
                                <tr key={u.id}>
                                    <td>{i + 1}</td>
                                    <td>{u.username}</td>
                                    <td>{u.fullName}</td>
                                    <td>{u.phone}</td>
                                    <td className="text-base">{u.email}</td>
                                    <td>
                                        <span className={`rounded border px-2 py-0.5 text-sm ${isAdmin ? 'border-red-300 bg-red-50 text-red-500' : 'border-blue-300 bg-blue-50 text-blue-500'}`}>
                                            {isAdmin ? 'Admin' : 'User'}
                                        </span>
                                    </td>
                                    <td className="whitespace-nowrap">
                                        <button
                                            disabled={isMe}
                                            onClick={() => toggleRole(u)}
                                            className="mr-2 rounded border border-gray-300 px-2 py-0.5 text-base hover:bg-gray-100 disabled:text-gray-300"
                                        >
                                            {isAdmin ? 'Hạ quyền' : 'Cấp admin'}
                                        </button>
                                        <button
                                            disabled={isAdmin}
                                            onClick={() => remove(u)}
                                            className="rounded border border-red-400 px-2 py-0.5 text-base text-red-500 hover:bg-red-50 disabled:border-gray-200 disabled:bg-gray-50 disabled:text-gray-300"
                                        >
                                            Xóa
                                        </button>
                                    </td>
                                </tr>
                            )
                        })}
                    </tbody>
                </table>
            </div>
        </div>
    )
}

export default UserManager
