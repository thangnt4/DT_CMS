import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { ApiResponse, PagedResult, QueryParams } from '../models/api-response.model';
import { SanPham } from '../models/san-pham.model';

@Injectable({ providedIn: 'root' })
export class SanPhamService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/sanpham`;

  getSanPhams(query: QueryParams = {}): Observable<ApiResponse<PagedResult<SanPham>>> {
    let params = new HttpParams();
    if (query.page) params = params.set('page', query.page);
    if (query.pageSize) params = params.set('pageSize', query.pageSize);
    if (query.search) params = params.set('search', query.search);
    return this.http.get<ApiResponse<PagedResult<SanPham>>>(this.apiUrl, { params });
  }

  getSanPhamById(id: number): Observable<ApiResponse<SanPham>> {
    return this.http.get<ApiResponse<SanPham>>(`${this.apiUrl}/${id}`);
  }

  createSanPham(SanPham: FormData): Observable<ApiResponse<SanPham>> {
    return this.http.post<ApiResponse<SanPham>>(this.apiUrl, SanPham);
  }

  updateSanPham(id: number, SanPham: FormData): Observable<ApiResponse<SanPham>> {
    return this.http.put<ApiResponse<SanPham>>(`${this.apiUrl}/${id}`, SanPham);
  }

  deleteSanPham(id: number): Observable<ApiResponse<void>> {
    return this.http.delete<ApiResponse<void>>(`${this.apiUrl}/${id}`);
  }
}
