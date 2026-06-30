import { Routes } from '@angular/router';
import { authGuard } from '../core/auth/auth.guard';
import { AdminLayoutComponent } from '../shared/components/layout/admin-layout.component';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  {
    path: 'login',
    loadComponent: () => import('../features/auth/login.component').then((m) => m.LoginComponent)
  },
  {
    path: '',
    component: AdminLayoutComponent,
    canActivate: [authGuard],
    children: [
      {
        path: 'dashboard',
        loadComponent: () => import('../features/dashboard/dashboard.component').then((m) => m.DashboardComponent)
      },
      {
        path: 'system',
        loadChildren: () => import('../features/system/system.routes').then((m) => m.systemRoutes)
      }
    ]
  },
  { path: '**', redirectTo: 'dashboard' }
];
