import { useState } from 'react'
import { reviewApi } from '../../api'
import type { Review, ReviewStatus } from '../../api'
import { useApi } from '../../hooks/useApi'
import { useStore } from '../../context/StoreContext'
import { errorText, formatDate, reviewStatusLabel } from '../../utils/format'
import { ErrorBox, Loading } from '../../components/Ui'

const statusStyle: Record<ReviewStatus, string> = {
    ChoDuyet: 'border-amber-300 bg-amber-50 text-amber-600',
    DaDuyet: 'border-green-300 bg-green-50 text-green-600',
    KhongHopLe: 'border-gray-300 bg-gray-50 text-gray-500',
}

const ReviewManager = () => {
    const { notify } = useStore()
    const { data, loading, error, reload } = useApi(() => reviewApi.all(), [])
    const [filter, setFilter] = useState<ReviewStatus | 'all'>('ChoDuyet')

    const act = async (fn: () => Promise<string>) => {
        try {
            notify(await fn())
            await reload()
        } catch (e) {
            notify(errorText(e), 'error')
        }
    }

    const remove = (r: Review) => {
        if (confirm('Xóa đánh giá này?')) act(() => reviewApi.remove(r.id))
    }

    if (loading && !data) return <Loading />
    if (error && !data) return <ErrorBox message={error} onRetry={reload} />

    const list = (data ?? []).filter(r => filter === 'all' || r.status === filter)

    return (
        <div>
            <div className="mb-6 flex flex-wrap items-center justify-between gap-4">
                <h1 className="text-3xl font-bold">Quản lý đánh giá sản phẩm</h1>
                <select value={filter} onChange={e => setFilter(e.target.value as ReviewStatus | 'all')} className="rounded border border-gray-300 px-3 py-2">
                    <option value="all">Tất cả</option>
                    {(Object.keys(reviewStatusLabel) as ReviewStatus[]).map(s => <option key={s} value={s}>{reviewStatusLabel[s]}</option>)}
                </select>
            </div>
            {list.length === 0 ? (
                <p className="rounded bg-white p-10 text-center text-gray-500">Không có đánh giá nào.</p>
            ) : (
                <div className="overflow-x-auto">
                    <table className="admin-table text-lg">
                        <thead>
                            <tr><th>STT</th><th>SẢN PHẨM</th><th>NGƯỜI ĐÁNH GIÁ</th><th>SỐ SAO</th><th>BÌNH LUẬN</th><th>NGÀY</th><th>TRẠNG THÁI</th><th>HÀNH ĐỘNG</th></tr>
                        </thead>
                        <tbody>
                            {list.map((r, i) => (
                                <tr key={r.id}>
                                    <td>{i + 1}</td>
                                    <td className="!text-left">{r.productName}</td>
                                    <td>{r.userName}</td>
                                    <td className="whitespace-nowrap text-amber-400">{'★'.repeat(r.stars)}</td>
                                    <td className="!text-left text-base">{r.comment}</td>
                                    <td>{formatDate(r.date)}</td>
                                    <td>
                                        <span className={`whitespace-nowrap rounded border px-2 py-0.5 text-sm ${statusStyle[r.status]}`}>
                                            {reviewStatusLabel[r.status] ?? r.status}
                                        </span>
                                    </td>
                                    <td className="whitespace-nowrap">
                                        {r.status !== 'DaDuyet' && (
                                            <button onClick={() => act(() => reviewApi.setStatus(r.id, 'DaDuyet'))} className="mr-2 rounded bg-green-600 px-2 py-0.5 text-white hover:bg-green-700">Duyệt</button>
                                        )}
                                        {r.status !== 'KhongHopLe' && (
                                            <button onClick={() => act(() => reviewApi.setStatus(r.id, 'KhongHopLe'))} className="mr-2 rounded border border-gray-400 px-2 py-0.5 hover:bg-gray-100">Từ chối</button>
                                        )}
                                        <button onClick={() => remove(r)} className="rounded border border-red-400 px-2 py-0.5 text-red-500 hover:bg-red-50">Xóa</button>
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
            )}
        </div>
    )
}

export default ReviewManager
