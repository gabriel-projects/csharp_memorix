const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export interface ApiResponse<T> {
    success: boolean;
    data?: T;
    error?: string;
}

class ApiClient {
    private baseURL: string;

    constructor(baseURL: string) {
        this.baseURL = baseURL;
    }

    private async request<T>(endpoint: string, options?: RequestInit = {}): Promise<ApiResponse<T>> {
        const url = `${this.baseURL}${endpoint}`;

        const token = localStorage.getItem('accessToken');

        const headers: HeadersInit = {
            'Content-Type': 'application/json',
            ...(token ? { 'Authorization': `Bearer ${token}` } : {}),
            ...options.headers,
        };

        try {
            const response = await fetch(url, {...options, headers });

            const data = await response.json();

            if (!response.ok) {
                return { success: false, error: data.message || 'API request failed' };
            }

            return {
                success: true,
                data: data.data || data
            }
        } catch (error) {
            return { success: false, error: (error as Error).message };
        }
    }

    public get<T>(endpoint: string): Promise<ApiResponse<T>> {
        return this.request<T>(endpoint, {
            method: 'GET',
        });
    }

    async post<T>(endpoint: string, body?: unknown): Promise<ApiResponse<T>> {
        return this.request<T>(endpoint, {
            method: 'POST',
            body: JSON.stringify(body),
        });
    }

    async put<T>(endpoint: string, body?: unknown): Promise<ApiResponse<T>> {
        return this.request<T>(endpoint, {
            method: 'PUT',
            body: JSON.stringify(body),
        });
    }

    async delete<T>(endpoint: string): Promise<ApiResponse<T>> {
        return this.request<T>(endpoint, { method: 'DELETE' });
    }
}

export const apiClient = new ApiClient(API_BASE_URL);