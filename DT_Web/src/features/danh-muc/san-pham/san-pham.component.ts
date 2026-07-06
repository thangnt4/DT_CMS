import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';

import { TableModule, TableLazyLoadEvent } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { TextareaModule } from 'primeng/textarea';
import { DialogModule } from 'primeng/dialog';
import { ToggleSwitchModule } from 'primeng/toggleswitch';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { TagModule } from 'primeng/tag';
import { ConfirmationService, MessageService } from 'primeng/api';

import { SanPhamService } from '../../../core/services/san-pham.service';
import { SanPham } from '../../../core/models/san-pham.model';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-san-pham',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    TableModule,
    ButtonModule,
    InputTextModule,
    TextareaModule,
    DialogModule,
    ToggleSwitchModule,
    IconFieldModule,
    InputIconModule,
    TagModule
  ],
  templateUrl: './san-pham.component.html'
})
export class SanPhamComponent {
  private readonly sanPhamService = inject(SanPhamService);
  private readonly fb = inject(FormBuilder);
  private readonly messageService = inject(MessageService);
  private readonly confirmationService = inject(ConfirmationService);

  readonly items = signal<SanPham[]>([]);
  readonly totalRecords = signal(0);
  readonly loading = signal(false);
  readonly dialogVisible = signal(false);
  readonly isEditMode = signal(false);
  readonly saving = signal(false);
  readonly selectedFile = signal<File | null>(null);
  readonly currentFileDinhKem = signal<string | null>(null);
  readonly fileBaseUrl = environment.apiUrl.replace(/\/api$/, '');

  keyword = '';
  page = 1;
  pageSize = 10;

  private editingId: number | null = null;

  readonly form = this.fb.nonNullable.group({
    tenDuLieu: ['', Validators.required],
    ghiChu: [''],
    trangThai: [true]
  });

  // p-table with [lazy]="true" fires onLazyLoad once on first render,
  // which triggers the initial load() — no explicit call needed here.

  search(): void {
    this.page = 1;
    this.load();
  }

  load(): void {
    this.loading.set(true);
    this.sanPhamService.getSanPhams({ search: this.keyword, page: this.page, pageSize: this.pageSize }).subscribe({
      next: (res) => {
        this.loading.set(false);
        if (res.success && res.data) {
          this.items.set(res.data.items);
          this.totalRecords.set(res.data.totalCount);
        }
      },
      error: () => {
        this.loading.set(false);
        this.messageService.add({ severity: 'error', summary: 'Lỗi', detail: 'Không thể tải danh sách sản phẩm.' });
      }
    });
  }

  onPage(event: TableLazyLoadEvent): void {
    const rows = event.rows ?? this.pageSize;
    this.pageSize = rows;
    this.page = Math.floor((event.first ?? 0) / rows) + 1;
    this.load();
  }

  openCreateDialog(): void {
    this.isEditMode.set(false);
    this.editingId = null;
    this.selectedFile.set(null);
    this.currentFileDinhKem.set(null);
    this.form.reset({ tenDuLieu: '', ghiChu: '', trangThai: true });
    this.dialogVisible.set(true);
  }

  openEditDialog(item: SanPham): void {
    this.isEditMode.set(true);
    this.editingId = item.id;
    this.selectedFile.set(null);
    this.currentFileDinhKem.set(item.fileDinhKem ?? null);
    this.form.reset({
      tenDuLieu: item.tenDuLieu,
      ghiChu: item.ghiChu ?? '',
      trangThai: item.trangThai === 1
    });
    this.dialogVisible.set(true);
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.selectedFile.set(input.files?.length ? input.files[0] : null);
  }

  closeDialog(): void {
    this.dialogVisible.set(false);
  }

  save(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.saving.set(true);
    const raw = this.form.getRawValue();
    const formData = new FormData();
    formData.append('tenDuLieu', raw.tenDuLieu);
    if (raw.ghiChu) formData.append('ghiChu', raw.ghiChu);
    formData.append('trangThai', raw.trangThai ? '1' : '0');
    if (this.selectedFile()) {
      formData.append('file', this.selectedFile()!);
    }

    const request$ = this.isEditMode()
      ? this.sanPhamService.updateSanPham(this.editingId!, formData)
      : this.sanPhamService.createSanPham(formData);

    request$.subscribe({
      next: () => {
        this.saving.set(false);
        this.dialogVisible.set(false);
        this.messageService.add({
          severity: 'success',
          summary: 'Thành công',
          detail: this.isEditMode() ? 'Cập nhật sản phẩm thành công.' : 'Thêm sản phẩm thành công.'
        });
        this.load();
      },
      error: (err) => {
        this.saving.set(false);
        this.messageService.add({
          severity: 'error',
          summary: 'Lỗi',
          detail: err?.error?.message ?? 'Có lỗi xảy ra, vui lòng thử lại.'
        });
      }
    });
  }

  confirmDelete(item: SanPham): void {
    this.confirmationService.confirm({
      header: 'Xác nhận xóa',
      message: `Bạn có chắc chắn muốn xóa sản phẩm "${item.tenDuLieu}"?`,
      icon: 'pi pi-exclamation-triangle',
      acceptLabel: 'Xóa',
      rejectLabel: 'Hủy',
      acceptButtonStyleClass: 'p-button-danger',
      accept: () => this.deleteSanPham(item)
    });
  }

  private deleteSanPham(item: SanPham): void {
    this.sanPhamService.deleteSanPham(item.id).subscribe({
      next: () => {
        this.messageService.add({ severity: 'success', summary: 'Thành công', detail: 'Đã xóa sản phẩm.' });
        this.load();
      },
      error: (err) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Lỗi',
          detail: err?.error?.message ?? 'Không thể xóa sản phẩm.'
        });
      }
    });
  }
}
