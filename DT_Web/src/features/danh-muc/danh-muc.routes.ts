import { Routes } from '@angular/router';

export const danhMucRoutes: Routes = [
  {
    path: 'chuc-vu',
    loadComponent: () => import('./chuc-vu/chuc-vu.component').then((m) => m.ChucVuComponent)
  },
  {
    path: 'danh-muc-san-pham',
    loadComponent: () => import('./san-pham/san-pham.component').then((m) => m.SanPhamComponent)
  }
];
