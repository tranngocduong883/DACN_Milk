import { useState } from 'react'
import { storeApi } from '../api'
import { useApi } from '../hooks/useApi'
import { ErrorBox, Loading } from '../components/Ui'
import { PhoneIcon } from '../components/Icons'

const Stores = () => {
    const { data, loading, error, reload } = useApi(() => storeApi.all(), [])
    const [keyword, setKeyword] = useState('')

    const k = keyword.trim().toLowerCase()
    const list = (data ?? []).filter(s => !k || `${s.name} ${s.address}`.toLowerCase().includes(k))

    return (
        <section className="mx-auto max-w-[1400px] px-4 py-10 md:px-10">
            <div className="mb-6 flex flex-wrap items-center justify-between gap-4">
                <h1 className="text-3xl font-bold">Danh sách cửa hàng</h1>
                <input
                    value={keyword}
                    onChange={e => setKeyword(e.target.value)}
                    placeholder="Tìm theo tên hoặc khu vực (Hà Nội, Đà Nẵng...)"
                    className="w-full max-w-md rounded-full border border-gray-300 px-5 py-2.5 outline-none focus:border-sky-dark"
                />
            </div>
            {loading ? <Loading /> : error ? <ErrorBox message={error} onRetry={reload} /> : list.length === 0 ? (
                <p className="rounded-lg bg-white p-10 text-center text-gray-500">Không tìm thấy cửa hàng phù hợp.</p>
            ) : (
                <div className="grid gap-6 sm:grid-cols-2 lg:grid-cols-3">
                    {list.map(s => (
                        <div key={s.id} className="rounded-lg bg-white p-6 shadow-sm">
                            <h2 className="text-lg font-semibold text-navy">{s.name}</h2>
                            <p className="mt-2 text-gray-700">{s.address}</p>
                            {s.phone && (
                                <a href={`tel:${s.phone}`} className="mt-3 flex items-center gap-2 text-brand hover:underline">
                                    <PhoneIcon size={16} /> {s.phone}
                                </a>
                            )}
                            {s.email && <p className="mt-1 text-sm text-gray-500">{s.email}</p>}
                            {s.description && <p className="mt-2 text-sm italic text-gray-500">{s.description}</p>}
                        </div>
                    ))}
                </div>
            )}
        </section>
    )
}

export default Stores
