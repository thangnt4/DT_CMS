import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { ApiResponse, PagedResult, QueryParams } from '../models/api-response.model';
import { TinTuc, CreateTinTuc, UpdateTinTuc } from '../models/tin-tuc.model';

@Injectable({ providedIn: 'root' })
export class TinTucService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = `${environment.apiUrl}/tintuc`;

  getTinTucs(query: QueryParams = {}): Observable<ApiResponse<PagedResult<TinTuc>>> {
    let params = new HttpParams();
    if (query.page) params = params.set('page', query.page);
    if (query.pageSize) params = params.set('pageSize', query.pageSize);
    if (query.search) params = params.set('search', query.search);
    return this.http.get<ApiResponse<PagedResult<TinTuc>>>(this.apiUrl, { params });
  }

  getTinTucById(id: number): Observable<ApiResponse<TinTuc>> {
    return this.http.get<ApiResponse<TinTuc>>(`${this.apiUrl}/${id}`);
  }

  createTinTuc(tinTuc: CreateTinTuc): Observable<ApiResponse<TinTuc>> {
    return this.http.post<ApiResponse<TinTuc>>(this.apiUrl, tinTuc);
  }

  updateTinTuc(id: number, tinTuc: UpdateTinTuc): Observable<ApiResponse<TinTuc>> {
    return this.http.put<ApiResponse<TinTuc>>(`${this.apiUrl}/${id}`, tinTuc);
  }

  deleteTinTuc(id: number): Observable<ApiResponse<void>> {
    return this.http.delete<ApiResponse<void>>(`${this.apiUrl}/${id}`);
  }
}
