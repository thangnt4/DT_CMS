import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { SidebarComponent } from './sidebar.component';
import { HeaderComponent } from './header.component';

@Component({
  selector: 'app-admin-layout',
  standalone: true,
  imports: [RouterOutlet, SidebarComponent, HeaderComponent],
  templateUrl: './admin-layout.component.html'
})
export class AdminLayoutComponent {
  isCollapsed = signal(false);

  toggleSidenav(): void {
    this.isCollapsed.update((value) => !value);
  }
}
