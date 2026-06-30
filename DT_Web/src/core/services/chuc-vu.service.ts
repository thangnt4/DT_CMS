import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { ApiResponse, PagedResult, QueryParams } from '../models/api-response.model';
import { ChucVu, CreateChucVu, UpdateChucVu } from '../models/chuc-vu.model';

@Injectable({ providedIn: 'root' })
export class ChucVuService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/chucvu`;

  getChucVus(query: QueryParams = {}): Observable<ApiResponse<PagedResult<ChucVu>>> {
    let params = new HttpParams();
    if (query.page) params = params.set('page', query.page);
    if (query.pageSize) params = params.set('pageSize', query.pageSize);
    if (query.search) params = params.set('search', query.search);
    return this.http.get<ApiResponse<PagedResult<ChucVu>>>(this.apiUrl, { params });
  }

  getChucVuById(id: number): Observable<ApiResponse<ChucVu>> {
    return this.http.get<ApiResponse<ChucVu>>(`${this.apiUrl}/${id}`);
  }

  createChucVu(chucVu: CreateChucVu): Observable<ApiResponse<ChucVu>> {
    return this.http.post<ApiResponse<ChucVu>>(this.apiUrl, chucVu);
  }

  updateChucVu(id: number, chucVu: UpdateChucVu): Observable<ApiResponse<ChucVu>> {
    return this.http.put<ApiResponse<ChucVu>>(`${this.apiUrl}/${id}`, chucVu);
  }

  deleteChucVu(id: number): Observable<ApiResponse<void>> {
    return this.http.delete<ApiResponse<void>>(`${this.apiUrl}/${id}`);
  }
}
