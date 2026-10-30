import { Link, useNavigate } from 'react-router-dom'
import type { Product } from '../api'
import { formatPrice } from '../utils/format'
import { useStore } from '../context/StoreContext'
import { CartIcon } from './Icons'

const ProductCard = ({ product }: { product: Product }) => {
    const { addToCart, notify } = useStore()
    const navigate = useNavigate()
    const onSale = product.price < product.originalPrice

    const onAdd = async () => {
        try {
            if (!(await addToCart(product.id))) {
                notify('Vui lòng đăng nhập để thêm vào giỏ hàng', 'error')
                navigate('/login')
            }
        } catch {
            /* đã thông báo lỗi */
        }
    }

    return (
        <div className="relative flex flex-col overflow-hidden rounded-lg bg-white shadow-sm transition hover:shadow-md">
            {onSale && (
                <span className="absolute left-3 top-3 rounded bg-price px-2 py-0.5 text-sm font-semibold text-white">
                    -{Math.round((1 - product.price / product.originalPrice) * 100)}%
                </span>
            )}
            <Link to={`/product/${product.id}`} className="block aspect-square bg-white">
                <img src={product.image} alt={product.name} className="size-full object-contain" />
            </Link>
            <div className="flex flex-1 flex-col p-4">
                <Link to={`/product/${product.id}`} className="min-h-[3.25rem] text-[17px] leading-snug hover:text-brand">
                    {product.name}
                </Link>
                <p className="mt-auto pt-5 text-lg font-bold text-price">
                    {formatPrice(product.price)}
                    {onSale && <span className="ml-2 text-sm font-normal text-gray-400 line-through">{formatPrice(product.originalPrice)}</span>}
                </p>
                <button
                    onClick={onAdd}
                    className="mt-3 flex items-center justify-center gap-2 rounded-md bg-brand py-3 text-lg text-white hover:bg-brand-dark"
                >
                    <CartIcon size={18} /> Thêm vào giỏ
                </button>
            </div>
        </div>
    )
}

export default ProductCard
