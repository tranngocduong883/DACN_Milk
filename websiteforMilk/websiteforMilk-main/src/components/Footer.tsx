import { FacebookIcon, YoutubeIcon } from './Icons'

const Footer = () => (
    <footer id="footer" className="bg-sky text-white">
        <div className="grid gap-10 px-5 py-12 sm:grid-cols-2 lg:grid-cols-4">
            <div>
                <h3 className="mb-6 text-xl uppercase">Hạnh phúc đích thực</h3>
                <div className="flex gap-5">
                    <a href="#" aria-label="Facebook" className="grid place-items-center size-12 rounded-full border border-white hover:bg-white/20">
                        <FacebookIcon size={20} />
                    </a>
                    <a href="#" aria-label="Youtube" className="grid place-items-center size-12 rounded-full border border-white hover:bg-white/20">
                        <YoutubeIcon size={20} />
                    </a>
                </div>
            </div>
            <div>
                <h3 className="mb-4 text-xl font-semibold uppercase">Liên hệ</h3>
                <p className="text-lg leading-relaxed">
                    Cơ quan chủ quản: CTCP Chuỗi Thực Phẩm TH MST: 2901270911 - Số ĐKKD: 2901270911 cấp ngày 06/09/2010
                    bởi Sở KH và ĐT tỉnh Nghệ An Số 166 đường Nguyễn Thái Học, Phường Thành Vinh, Tỉnh Nghệ An, Việt Nam
                    <br />1800 545440
                    <br />chamsockhachhang@thmilk.vn
                </p>
            </div>
            <div>
                <h3 className="mb-4 text-xl font-semibold uppercase">Hỗ trợ khách hàng</h3>
                <ul className="space-y-2 text-lg">
                    {['Bảo mật thông tin', 'Chính sách giao hàng', 'Điều khoản sử dụng', 'Hướng dẫn đặt hàng'].map(t => (
                        <li key={t}><a href="#" className="hover:underline">— {t}</a></li>
                    ))}
                </ul>
            </div>
            <div>
                <h3 className="mb-4 text-xl font-semibold uppercase">Mã khuyến mãi</h3>
                <ul className="space-y-2 text-lg uppercase">
                    <li><a href="#" className="hover:underline">Chương trình khuyến mãi</a></li>
                    <li><a href="#" className="hover:underline">Khảo sát</a></li>
                </ul>
            </div>
        </div>
        <div className="border-t border-white/30 py-6 text-center">
            Copyright @ 2020 CTCP Chuỗi Thực phẩm TH. All Rights Reserved.
        </div>
    </footer>
)

export default Footer
