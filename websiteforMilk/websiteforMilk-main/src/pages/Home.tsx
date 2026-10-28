import { useSearchParams } from 'react-router-dom'
import { productApi } from '../api'
import { useApi } from '../hooks/useApi'
import { useStore } from '../context/StoreContext'
import ProductCard from '../components/ProductCard'
import { ErrorBox, Loading } from '../components/Ui'

const Home = () => {
    const [params] = useSearchParams()
    const { categories } = useStore()
    const { data: products, loading, error, reload } = useApi(() => productApi.all(), [])

    const rawQ = params.get('q') ?? ''
    const q = rawQ.toLowerCase()
    const categoryId = params.get('category')
    const onlyNew = params.get('new') === '1'

    let list = (products ?? []).filter(p =>
        (!q || p.name.toLowerCase().includes(q)) &&
        (!categoryId || p.categoryId === categoryId),
    )
    // Không có ngày tạo nên "sản phẩm mới" là các sản phẩm được thêm sau cùng
    if (onlyNew) list = list.slice(-8).reverse()

    const title = q
        ? `Kết quả tìm kiếm cho "${rawQ}"`
        : categoryId
            ? categories.find(c => c.id === categoryId)?.name ?? 'Sản phẩm'
            : onlyNew ? 'Sản phẩm mới' : 'Sản phẩm nổi bật'

    const emptyText = categoryId && !q ? 'Không có sản phẩm trong danh mục này' : 'Không tìm thấy sản phẩm phù hợp.'

    return (
        <>
            {!q && !categoryId && !onlyNew && (
                <div className="px-3 pt-10 md:px-5">
                    <img src="/images/banner.png" alt="NutriMilk - Sale 50%" className="block w-full" />
                </div>
            )}

            <section className="mx-auto max-w-[1600px] px-4 py-10 md:px-[170px]">
                <h2 className="mb-6 text-2xl font-medium">{title}</h2>
                {loading ? (
                    <Loading />
                ) : error ? (
                    <ErrorBox message={error} onRetry={reload} />
                ) : list.length === 0 ? (
                    <p className="rounded-lg bg-white p-10 text-center text-gray-500">{emptyText}</p>
                ) : (
                    <div className="grid grid-cols-1 gap-8 sm:grid-cols-2 lg:grid-cols-4">
                        {list.map(p => <ProductCard key={p.id} product={p} />)}
                    </div>
                )}
            </section>
        </>
    )
}

export default Home
