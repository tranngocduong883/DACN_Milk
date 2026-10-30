import { useEffect } from 'react'
import type { InputHTMLAttributes, ReactNode, SelectHTMLAttributes, TextareaHTMLAttributes } from 'react'

export const Loading = ({ text = 'Đang tải dữ liệu...' }: { text?: string }) => (
    <div className="flex items-center justify-center gap-3 p-10 text-gray-500">
        <span className="size-5 animate-spin rounded-full border-2 border-sky-dark border-t-transparent" />
        {text}
    </div>
)

export const ErrorBox = ({ message, onRetry }: { message: string; onRetry?: () => void }) => (
    <div className="rounded-lg border border-red-200 bg-red-50 p-6 text-center text-red-600">
        <p>{message}</p>
        {onRetry && (
            <button onClick={onRetry} className="mt-3 rounded border border-red-300 px-4 py-1.5 hover:bg-red-100">Thử lại</button>
        )}
    </div>
)

export const Modal = ({ title, onClose, children, wide }: { title: string; onClose: () => void; children: ReactNode; wide?: boolean }) => {
    useEffect(() => {
        const onKey = (e: KeyboardEvent) => e.key === 'Escape' && onClose()
        window.addEventListener('keydown', onKey)
        return () => window.removeEventListener('keydown', onKey)
    }, [onClose])

    return (
        <div className="fixed inset-0 z-50 flex items-start justify-center overflow-y-auto bg-black/40 p-4 md:p-10" onMouseDown={onClose}>
            <div
                role="dialog"
                aria-modal="true"
                className={`w-full rounded-lg bg-white shadow-xl ${wide ? 'max-w-3xl' : 'max-w-lg'}`}
                onMouseDown={e => e.stopPropagation()}
            >
                <div className="flex items-center justify-between border-b border-gray-200 px-6 py-4">
                    <h2 className="text-xl font-semibold">{title}</h2>
                    <button onClick={onClose} className="text-2xl leading-none text-gray-400 hover:text-gray-700" aria-label="Đóng">×</button>
                </div>
                <div className="p-6">{children}</div>
            </div>
        </div>
    )
}

const fieldCls = 'w-full rounded border border-gray-300 px-3 py-2 outline-none focus:border-sky-dark disabled:bg-gray-100'

interface FieldProps { label: string }

export const Field = ({ label, ...rest }: FieldProps & InputHTMLAttributes<HTMLInputElement>) => (
    <label className="block">
        <span className="mb-1 block font-medium">{label} {rest.required && <span className="text-red-500">*</span>}</span>
        <input {...rest} className={fieldCls} />
    </label>
)

export const TextArea = ({ label, ...rest }: FieldProps & TextareaHTMLAttributes<HTMLTextAreaElement>) => (
    <label className="block">
        <span className="mb-1 block font-medium">{label}</span>
        <textarea {...rest} className={`${fieldCls} h-24`} />
    </label>
)

export const Select = ({ label, children, ...rest }: FieldProps & SelectHTMLAttributes<HTMLSelectElement>) => (
    <label className="block">
        <span className="mb-1 block font-medium">{label} {rest.required && <span className="text-red-500">*</span>}</span>
        <select {...rest} className={fieldCls}>{children}</select>
    </label>
)

export const FormActions = ({ onCancel, saving, submitText = 'Lưu' }: { onCancel: () => void; saving?: boolean; submitText?: string }) => (
    <div className="flex justify-end gap-3 pt-2">
        <button type="button" onClick={onCancel} className="rounded border border-gray-300 px-5 py-2 hover:bg-gray-100">Hủy</button>
        <button disabled={saving} className="rounded bg-blue-600 px-5 py-2 text-white hover:bg-blue-700 disabled:opacity-60">
            {saving ? 'Đang lưu...' : submitText}
        </button>
    </div>
)
