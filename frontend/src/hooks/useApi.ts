import { useCallback, useEffect, useState } from 'react'
import type { DependencyList } from 'react'
import { errorText } from '../utils/format'

// Tải dữ liệu từ API, trả về { data, loading, error, reload }
export function useApi<T>(fn: () => Promise<T>, deps: DependencyList) {
    const [data, setData] = useState<T | null>(null)
    const [loading, setLoading] = useState(true)
    const [error, setError] = useState('')

    // eslint-disable-next-line react-hooks/exhaustive-deps
    const load = useCallback(fn, deps)

    const reload = useCallback(async () => {
        setLoading(true)
        setError('')
        try {
            setData(await load())
        } catch (e) {
            setError(errorText(e))
        } finally {
            setLoading(false)
        }
    }, [load])

    useEffect(() => { reload() }, [reload])

    return { data, setData, loading, error, reload }
}
