import type { InputHTMLAttributes } from 'react'

interface Props extends InputHTMLAttributes<HTMLInputElement> {
    label: string
}

// Ô nhập bo tròn dùng cho trang Đăng nhập / Đăng ký
const AuthField = ({ label, required, ...rest }: Props) => (
    <label className="block">
        <span className="mb-2 block font-semibold uppercase">
            {label} {required && <span className="text-red-500">*</span>}
        </span>
        <input
            required={required}
            {...rest}
            className="w-full rounded-full border border-gray-900 px-5 py-3 outline-none focus:border-cyan focus:ring-2 focus:ring-cyan/30"
        />
    </label>
)

export default AuthField
