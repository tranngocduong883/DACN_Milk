import { Link } from 'react-router-dom'
import type { ComponentType } from 'react'
import { categoryApi, orderApi, productApi, storeApi, userApi, voucherApi } from '../../api'
import { useApi } from '../../hooks/useApi'
import { formatDate, formatPriceShort } from '../../utils/format'
import { ErrorBox, Loading } from '../../components/Ui'
import { BagIcon, BoxIcon, ChevronRightIcon, GiftIcon, TagIcon, TruckIcon, UsersIcon } from '../../components/Icons'

interface Stat {
    label: string
    sub: string
    value: number
    to: string
    icon: ComponentType<{ size?: number }>
    color: string // màu chữ + viền
    bg: string // nền icon
}

const StatCard = ({ s }: { s: Stat }) => {
    const Icon = s.icon
    const [text, border] = s.color.split(' ')
    return (
        <Link to={s.to} className={`flex items-center gap-4 rounded-lg border border-l-4 bg-white p-5 transition hover:shadow-md ${border}`}>
            <span className={`grid size-12 place-items-center rounded-lg ${s.bg} ${text}`}><Icon size={24} /></span>
            <div className="flex-1">
                <p className="text-lg font-medium">{s.label}</p>
                <p className="text-gray-500">{s.sub}</p>
            </div>
            <span className={`text-3xl font-bold ${text}`}>{s.value}</span>
            <ChevronRightIcon size={20} />
        </Link>
    )
}

const dayKey = (d: Date) => `${d.getFullYear()}-${d.getMonth()}-${d.getDate()}`

const Dashboard = () => {
    const { data, loading, error, reload } = useApi(async () => {
        const [products, categories, orders, vouchers, stores, users] = await Promise.all([
            productApi.all(), categoryApi.all(), orderApi.all(), voucherApi.all(), storeApi.all(), userApi.all(),
        ])
        return { products, categories, orders, vouchers, stores, users }
    }, [])

    if (loading) return <Loading />
    if (error || !data) return <ErrorBox message={error} onRetry={reload} />

    const { products, categories, orders, vouchers, stores, users } = data

    const left: Stat[] = [
        { label: 'Sản phẩm', sub: 'Tổng số sản phẩm', value: products.length, to: '/admin/products', icon: BoxIcon, color: 'text-blue-600 border-gray-200', bg: 'bg-blue-100' },
        { label: 'Danh mục', sub: 'Tổng số danh mục', value: categories.length, to: '/admin/categories', icon: TagIcon, color: 'text-purple-600 border-purple-500', bg: 'bg-purple-100' },
        { label: 'Đơn hàng', sub: 'Tổng đơn hàng', value: orders.length, to: '/admin/orders', icon: BagIcon, color: 'text-green-600 border-green-500', bg: 'bg-green-100' },
    ]
    const right: Stat[] = [
        { label: 'Voucher', sub: 'Tổng số voucher', value: vouchers.length, to: '/admin/vouchers', icon: GiftIcon, color: 'text-pink-600 border-pink-500', bg: 'bg-pink-100' },
        { label: 'Cửa hàng', sub: 'Tổng số cửa hàng', value: stores.length, to: '/stores', icon: TruckIcon, color: 'text-orange-500 border-gray-200', bg: 'bg-orange-100' },
        { label: 'Tài khoản', sub: 'Tổng số tài khoản', value: users.length, to: '/admin/users', icon: UsersIcon, color: 'text-orange-600 border-orange-500', bg: 'bg-orange-100' },
    ]

    // Doanh thu 6 ngày gần nhất (không tính đơn đã hủy)
    const valid = orders.filter(o => o.status !== 'DaHuy')
    const days = Array.from({ length: 6 }, (_, i) => {
        const d = new Date()
        d.setHours(0, 0, 0, 0)
        d.setDate(d.getDate() - (5 - i))
        return d
    })
    const revenue = days.map(d => valid
        .filter(o => dayKey(new Date(o.date)) === dayKey(d))
        .reduce((s, o) => s + Math.max(0, o.total), 0))
    const total = revenue.reduce((a, b) => a + b, 0)
    const max = Math.max(...revenue, 1)

    // Sản phẩm bán chạy theo số lượng
    const sold = new Map<string, { name: string; qty: number; amount: number }>()
    valid.forEach(o => o.items.forEach(i => {
        const cur = sold.get(i.productId) ?? { name: i.name, qty: 0, amount: 0 }
        cur.qty += i.quantity
        cur.amount += i.quantity * i.price
        sold.set(i.productId, cur)
    }))
    const bestSellers = [...sold.entries()].sort((a, b) => b[1].qty - a[1].qty).slice(0, 5)

    return (
        <div>
            <h1 className="mb-8 flex items-center gap-3 text-3xl font-medium">📊 Bảng điều khiển</h1>

            <div className="grid gap-10 xl:grid-cols-2">
                {[left, right].map((col, i) => (
                    <div key={i} className="space-y-5 rounded-lg bg-white p-7 shadow-md">
                        {col.map(s => <StatCard key={s.label} s={s} />)}
                    </div>
                ))}
            </div>

            <div className="mt-12 rounded-lg border border-gray-200 bg-white p-7 shadow-md">
                <div className="mb-8 flex flex-wrap justify-between gap-2">
                    <h2 className="text-2xl font-medium">Doanh thu 6 ngày gần nhất</h2>
                    <p className="text-gray-600">
                        Tổng doanh thu: <b className="text-green-600">{total.toLocaleString('vi-VN')} <u>đ</u></b>
                    </p>
                </div>
                <div className="flex gap-3">
                    <div className="flex h-80 flex-col justify-between text-right text-sm text-gray-500">
                        {[1, 0.75, 0.5, 0.25, 0].map(t => <span key={t}>{formatPriceShort(Math.round(max * t))}</span>)}
                    </div>
                    <div className="flex-1">
                        <div className="relative flex h-80 items-end border-b border-l border-dashed border-gray-300">
                            {[0, 1, 2, 3].map(i => (
                                <div key={i} className="absolute inset-x-0 border-t border-dashed border-gray-200" style={{ top: `${i * 25}%` }} />
                            ))}
                            {revenue.map((r, i) => (
                                <div key={i} className="relative flex h-full flex-1 items-end justify-center border-r border-dashed border-gray-200 px-3">
                                    <div
                                        className="w-full max-w-44 rounded-t bg-[#1a86e2]"
                                        style={{ height: `${(r / max) * 100}%` }}
                                        title={`${r.toLocaleString('vi-VN')} đ`}
                                    />
                                </div>
                            ))}
                        </div>
                        <div className="flex">
                            {days.map((d, i) => <span key={i} className="flex-1 pt-2 text-center text-gray-600">{formatDate(d.toISOString())}</span>)}
                        </div>
                    </div>
                </div>
                {orders[0] && (
                    <p className="mt-4 text-sm text-gray-400">
                        Đơn gần nhất: {formatDate(orders[0].date)} · {formatPriceShort(Math.max(0, orders[0].total))}
                    </p>
                )}
            </div>

            <div className="mt-12 rounded-lg border border-gray-200 bg-white p-7 shadow-md">
                <h2 className="mb-5 text-2xl font-medium">Sản phẩm bán chạy</h2>
                {bestSellers.length === 0 ? (
                    <p className="text-gray-500">Chưa có dữ liệu bán hàng.</p>
                ) : (
                    <table className="admin-table">
                        <thead><tr><th>STT</th><th>Sản phẩm</th><th>Số lượng bán</th><th>Doanh thu</th></tr></thead>
                        <tbody>
                            {bestSellers.map(([id, s], i) => (
                                <tr key={id}>
                                    <td>{i + 1}</td>
                                    <td className="!text-left">{s.name}</td>
                                    <td>{s.qty}</td>
                                    <td className="font-semibold text-green-600">{formatPriceShort(s.amount)}</td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                )}
            </div>
        </div>
    )
}

export default Dashboard
