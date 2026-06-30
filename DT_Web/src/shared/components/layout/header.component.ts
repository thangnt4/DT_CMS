import { Component, EventEmitter, Output, inject } from '@angular/core';
import { ButtonModule } from 'primeng/button';
import { MenuModule } from 'primeng/menu';
import { TooltipModule } from 'primeng/tooltip';
import { MenuItem } from 'primeng/api';
import { AuthService } from '../../../core/auth/auth.service';
import { ThemeService } from '../../../core/services/theme.service';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [ButtonModule, MenuModule, TooltipModule],
  templateUrl: './header.component.html'
})
export class HeaderComponent {
  @Output() toggleSidenav = new EventEmitter<void>();

  readonly authService = inject(AuthService);
  readonly themeService = inject(ThemeService);

  userMenuItems: MenuItem[] = [
    {
      label: 'Đăng xuất',
      icon: 'pi pi-sign-out',
      command: () => this.authService.logout()
    }
  ];
}
