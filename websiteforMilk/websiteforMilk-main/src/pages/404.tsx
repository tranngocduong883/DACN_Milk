import { Link } from 'react-router-dom'

const NotFound = () => (
    <div className="px-4 py-16">
        <div className="mx-auto max-w-xl rounded-lg bg-white p-10 text-center shadow-md">
            <p className="text-7xl font-bold text-sky">404</p>
            <h1 className="mt-4 text-2xl font-semibold">Không tìm thấy trang</h1>
            <p className="mt-2 text-gray-500">Trang bạn tìm không tồn tại hoặc đã bị xóa.</p>
            <Link to="/" className="mt-8 inline-block rounded-full bg-cyan px-8 py-3 font-bold text-white hover:brightness-95">
                VỀ TRANG CHỦ
            </Link>
        </div>
    </div>
)

export default NotFound
