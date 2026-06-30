import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Subject, debounceTime } from 'rxjs';

import { TableModule, TableLazyLoadEvent } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';
import { PasswordModule } from 'primeng/password';
import { DialogModule } from 'primeng/dialog';
import { ToggleSwitchModule } from 'primeng/toggleswitch';
import { IconFieldModule } from 'primeng/iconfield';
import { InputIconModule } from 'primeng/inputicon';
import { TagModule } from 'primeng/tag';
import { ConfirmationService, MessageService } from 'primeng/api';

import { UserService } from '../../../core/services/user.service';
import { User } from '../../../core/models/user.model';

@Component({
  selector: 'app-users',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    TableModule,
    ButtonModule,
    InputTextModule,
    PasswordModule,
    DialogModule,
    ToggleSwitchModule,
    IconFieldModule,
    InputIconModule,
    TagModule
  ],
  templateUrl: './users.component.html'
})
export class UsersComponent implements OnInit {
  private readonly userService = inject(UserService);
  private readonly fb = inject(FormBuilder);
  private readonly messageService = inject(MessageService);
  private readonly confirmationService = inject(ConfirmationService);

  readonly users = signal<User[]>([]);
  readonly totalRecords = signal(0);
  readonly loading = signal(false);
  readonly dialogVisible = signal(false);
  readonly isEditMode = signal(false);
  readonly saving = signal(false);

  keyword = '';
  page = 1;
  pageSize = 10;

  private editingId: number | null = null;
  private readonly searchSubject = new Subject<string>();

  readonly form = this.fb.nonNullable.group({
    username: ['', [Validators.required, Validators.minLength(3)]],
    password: [''],
    fullName: ['', Validators.required],
    email: ['', [Validators.required, Validators.email]],
    isActive: [true]
  });

  ngOnInit(): void {
    // p-table with [lazy]="true" fires onLazyLoad once on first render,
    // which triggers the initial load() — no explicit call needed here.
    this.searchSubject.pipe(debounceTime(400)).subscribe(() => {
      this.page = 1;
      this.load();
    });
  }

  onSearchChange(value: string): void {
    this.keyword = value;
    this.searchSubject.next(value);
  }

  load(): void {
    this.loading.set(true);
    this.userService.getUsers({ search: this.keyword, page: this.page, pageSize: this.pageSize }).subscribe({
      next: (res) => {
        this.loading.set(false);
        if (res.success && res.data) {
          this.users.set(res.data.items);
          this.totalRecords.set(res.data.totalCount);
        }
      },
      error: () => {
        this.loading.set(false);
        this.messageService.add({ severity: 'error', summary: 'Lỗi', detail: 'Không thể tải danh sách người dùng.' });
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
    this.form.reset({ username: '', password: '', fullName: '', email: '', isActive: true });
    this.form.controls.username.enable();
    this.form.controls.password.addValidators([Validators.required, Validators.minLength(6)]);
    this.form.controls.password.updateValueAndValidity();
    this.dialogVisible.set(true);
  }

  openEditDialog(user: User): void {
    this.isEditMode.set(true);
    this.editingId = user.id;
    this.form.reset({
      username: user.username,
      password: '',
      fullName: user.fullName,
      email: user.email,
      isActive: user.isActive
    });
    this.form.controls.username.disable();
    this.form.controls.password.clearValidators();
    this.form.controls.password.addValidators(Validators.minLength(6));
    this.form.controls.password.updateValueAndValidity();
    this.dialogVisible.set(true);
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

    const request$ = this.isEditMode()
      ? this.userService.updateUser(this.editingId!, {
          fullName: raw.fullName,
          email: raw.email,
          isActive: raw.isActive,
          password: raw.password || null
        })
      : this.userService.createUser({
          username: raw.username,
          password: raw.password,
          fullName: raw.fullName,
          email: raw.email,
          isActive: raw.isActive
        });

    request$.subscribe({
      next: () => {
        this.saving.set(false);
        this.dialogVisible.set(false);
        this.messageService.add({
          severity: 'success',
          summary: 'Thành công',
          detail: this.isEditMode() ? 'Cập nhật người dùng thành công.' : 'Thêm người dùng thành công.'
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

  confirmDelete(user: User): void {
    this.confirmationService.confirm({
      header: 'Xác nhận xóa',
      message: `Bạn có chắc chắn muốn xóa người dùng "${user.fullName}" (${user.username})?`,
      icon: 'pi pi-exclamation-triangle',
      acceptLabel: 'Xóa',
      rejectLabel: 'Hủy',
      acceptButtonStyleClass: 'p-button-danger',
      accept: () => this.deleteUser(user)
    });
  }

  private deleteUser(user: User): void {
    this.userService.deleteUser(user.id).subscribe({
      next: () => {
        this.messageService.add({ severity: 'success', summary: 'Thành công', detail: 'Đã xóa người dùng.' });
        this.load();
      },
      error: (err) => {
        this.messageService.add({
          severity: 'error',
          summary: 'Lỗi',
          detail: err?.error?.message ?? 'Không thể xóa người dùng.'
        });
      }
    });
  }
}
