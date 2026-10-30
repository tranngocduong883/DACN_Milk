import { Routes, Route } from 'react-router-dom'
import Home from './pages/Home'
import Detail from './pages/Detail'
import Login from './pages/Login'
import Register from './pages/Register'
import Cart from './pages/Cart'
import Checkout from './pages/Checkout'
import OrderSuccess from './pages/OrderSuccess'
import Profile from './pages/Profile'
import ProfileOrders from './pages/ProfileOrders'
import ProfileAddress from './pages/ProfileAddress'
import ProfilePassword from './pages/ProfilePassword'
import Stores from './pages/Stores'
import NotFound from './pages/404'
import Dashboard from './pages/Admin/Dashboard'
import UserManager from './pages/Admin/UserManager'
import ProductManager from './pages/Admin/ProductManager'
import OrderManager from './pages/Admin/OrderManager'
import CategoryManager from './pages/Admin/CategoryManager'
import VoucherManager from './pages/Admin/VoucherManager'
import ReviewManager from './pages/Admin/ReviewManager'
import UserLayout from './layouts/UserLayout'
import ProfileLayout from './layouts/ProfileLayout'
import AdminLayout from './layouts/AdminLayout'

function App() {
    return (
        <Routes>
            {/* Layout dành cho người dùng */}
            <Route path="/" element={<UserLayout />}>
                <Route index element={<Home />} />
                <Route path="product/:id" element={<Detail />} />
                <Route path="login" element={<Login />} />
                <Route path="register" element={<Register />} />
                <Route path="cart" element={<Cart />} />
                <Route path="checkout" element={<Checkout />} />
                <Route path="order-success" element={<OrderSuccess />} />
                <Route path="stores" element={<Stores />} />
                <Route path="profile" element={<ProfileLayout />}>
                    <Route index element={<Profile />} />
                    <Route path="orders" element={<ProfileOrders />} />
                    <Route path="address" element={<ProfileAddress />} />
                    <Route path="password" element={<ProfilePassword />} />
                </Route>
                <Route path="*" element={<NotFound />} />
            </Route>

            {/* Layout dành cho admin */}
            <Route path="/admin" element={<AdminLayout />}>
                <Route index element={<Dashboard />} />
                <Route path="users" element={<UserManager />} />
                <Route path="products" element={<ProductManager />} />
                <Route path="orders" element={<OrderManager />} />
                <Route path="categories" element={<CategoryManager />} />
                <Route path="vouchers" element={<VoucherManager />} />
                <Route path="reviews" element={<ReviewManager />} />
            </Route>
        </Routes>
    )
}

export default App
