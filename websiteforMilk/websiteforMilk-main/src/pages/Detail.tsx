import { useState } from 'react'
import type { FormEvent } from 'react'
import { Link, useNavigate, useParams } from 'react-router-dom'
import { productApi, reviewApi } from '../api'
import { useApi } from '../hooks/useApi'
import { useStore } from '../context/StoreContext'
import { errorText, formatDate, formatPrice } from '../utils/format'
import { ErrorBox, Loading } from '../components/Ui'
import NotFound from './404'

const Stars = ({ value }: { value: number }) => (
    <span className="text-amber-400" aria-label={`${value} sao`}>
        {'★'.repeat(value)}<span className="text-gray-300">{'★'.repeat(5 - value)}</span>
    </span>
)

const ReviewSection = ({ productId }: { productId: string }) => {
    const { session, notify } = useStore()
    const { data: reviews, loading } = useApi(() => reviewApi.byProduct(productId), [productId])
    const [stars, setStars] = useState(5)
    const [comment, setComment] = useState('')
    const [sending, setSending] = useState(false)

    const avg = reviews?.length ? reviews.reduce((s, r) => s + r.stars, 0) / reviews.length : 0

    const onSubmit = async (e: FormEvent) => {
        e.preventDefault()
        if (!session) return
        setSending(true)
        try {
            notify(await reviewApi.create({ maSp: productId, maTaiKhoan: session.userId, soSao: stars, binhLuan: comment.trim() }))
            setComment('')
        } catch (err) {
            notify(errorText(err), 'error')
        } finally {
            setSending(false)
        }
    }

    return (
        <div className="mt-8 bg-white p-4 lg:p-6">
            <h2 className="text-2xl font-bold">Đánh giá sản phẩm</h2>
            {reviews && reviews.length > 0 && (
                <p className="mt-1 text-gray-600"><Stars value={Math.round(avg)} /> {avg.toFixed(1)}/5 · {reviews.length} đánh giá</p>
            )}

            {session ? (
                <form onSubmit={onSubmit} className="mt-5 space-y-3 rounded-lg border border-gray-200 p-4">
                    <div className="flex items-center gap-2">
                        <span className="font-medium">Chấm điểm:</span>
                        {[1, 2, 3, 4, 5].map(n => (
                            <button
                                type="button"
                                key={n}
                                onClick={() => setStars(n)}
                                className={`text-2xl ${n <= stars ? 'text-amber-400' : 'text-gray-300'}`}
                                aria-label={`${n} sao`}
                            >
                                ★
                            </button>
                        ))}
                    </div>
                    <textarea
                        value={comment}
                        onChange={e => setComment(e.target.value)}
                        placeholder="Chia sẻ cảm nhận của bạn về sản phẩm"
                        className="h-20 w-full rounded border border-gray-300 px-3 py-2 outline-none focus:border-sky-dark"
                    />
                    <div className="flex flex-wrap items-center justify-between gap-2">
                        <p className="text-sm text-gray-500">Chỉ khách đã nhận hàng thành công mới đánh giá được. Đánh giá sẽ hiển thị sau khi được duyệt.</p>
                        <button disabled={sending} className="rounded bg-navy px-5 py-2 text-white hover:bg-navy-dark disabled:opacity-60">Gửi đánh giá</button>
                    </div>
                </form>
            ) : (
                <p className="mt-4 text-gray-600"><Link to="/login" className="text-brand hover:underline">Đăng nhập</Link> để đánh giá sản phẩm.</p>
            )}

            <div className="mt-6 divide-y divide-gray-100">
                {loading ? <Loading /> : !reviews?.length ? (
                    <p className="py-4 text-gray-500">Chưa có đánh giá nào cho sản phẩm này.</p>
                ) : reviews.map(r => (
                    <div key={r.id} className="py-4">
                        <p className="font-medium">{r.userName} <span className="ml-2 text-sm font-normal text-gray-400">{formatDate(r.date)}</span></p>
                        <Stars value={r.stars} />
                        {r.comment && <p className="mt-1 text-gray-700">{r.comment}</p>}
                    </div>
                ))}
            </div>
        </div>
    )
}

const Detail = () => {
    const { id = '' } = useParams()
    const navigate = useNavigate()
    const { addToCart, notify } = useStore()
    const { data: product, loading, error } = useApi(() => productApi.get(id), [id])
    const [qty, setQty] = useState(1)
    const [active, setActive] = useState(0)

    if (loading) return <Loading />
    if (error && !product) return error.includes('Không tìm thấy') ? <NotFound /> : <div className="p-8"><ErrorBox message={error} /></div>
    if (!product) return <NotFound />

    const onSale = product.price < product.originalPrice
    const outOfStock = product.stock <= 0

    const add = async (goToCart: boolean) => {
        try {
            if (!(await addToCart(product.id, qty))) {
                notify('Vui lòng đăng nhập để mua hàng', 'error')
                navigate('/login')
                return
            }
            if (goToCart) navigate('/cart')
        } catch {
            /* đã thông báo lỗi */
        }
    }

    return (
        <div className="px-2 py-8 md:px-2">
            <div className="grid gap-10 bg-white p-4 lg:grid-cols-2 lg:p-6">
                <div>
                    <div className="aspect-[4/3] w-full">
                        <img src={product.gallery[active] ?? product.image} alt={product.name} className="size-full object-contain" />
                    </div>
                    <div className="mt-6 flex gap-6">
                        {product.gallery.map((src, i) => (
                            <button
                                key={i}
                                onClick={() => setActive(i)}
                                className={`size-24 border-2 p-1 ${i === active ? 'border-blue-400' : 'border-transparent'}`}
                            >
                                <img src={src} alt="" className="size-full object-contain" />
                            </button>
                        ))}
                    </div>
                </div>

                <div>
                    <h1 className="text-3xl font-bold">{product.name}</h1>
                    <p className="mt-3 text-3xl font-medium text-orange-500">
                        {product.price.toLocaleString('vi-VN')} <span className="underline">đ</span>
                        {onSale && <span className="ml-3 text-xl text-gray-400 line-through">{formatPrice(product.originalPrice)}</span>}
                    </p>

                    <div className="mt-6 flex flex-wrap gap-5">
                        <div className="flex items-center bg-gray-100">
                            <button onClick={() => setQty(q => Math.max(1, q - 1))} className="px-6 py-4 hover:bg-gray-200">-</button>
                            <span className="w-14 text-center">{qty}</span>
                            <button onClick={() => setQty(q => q + 1)} className="px-6 py-4 hover:bg-gray-200">+</button>
                        </div>
                        <button
                            disabled={outOfStock}
                            onClick={() => add(true)}
                            className="bg-navy px-8 py-4 text-lg uppercase text-white hover:bg-navy-dark disabled:opacity-50"
                        >
                            Chọn mua
                        </button>
                        <button
                            disabled={outOfStock}
                            onClick={() => add(false)}
                            className="bg-blue-100 px-8 py-4 text-lg uppercase text-navy hover:bg-blue-200 disabled:opacity-50"
                        >
                            Thêm vào giỏ hàng
                        </button>
                    </div>
                    {outOfStock && <p className="mt-3 text-red-600">Sản phẩm tạm hết hàng</p>}

                    <div className="mt-8 space-y-2 text-gray-700">
                        <p><b>Danh mục:</b> {product.categoryName || 'Đang cập nhật'}</p>
                        <p><b>Thương hiệu:</b> NutriMilk</p>
                        <p><b>Xuất xứ:</b> Việt Nam</p>
                        {product.volume != null && <p><b>Thể tích:</b> {product.volume} ml</p>}
                        <p><b>Tồn kho:</b> {product.stock}</p>
                        <p><b>Mô tả:</b> {product.description || 'Đang cập nhật'}</p>
                    </div>
                    <p className="mt-6 text-sm text-gray-400">
                        Giá đã bao gồm VAT · {formatPrice(product.price)}{product.unit && ` / ${product.unit.toLowerCase()}`}
                    </p>
                </div>
            </div>

            <ReviewSection productId={product.id} />
        </div>
    )
}

export default Detail
