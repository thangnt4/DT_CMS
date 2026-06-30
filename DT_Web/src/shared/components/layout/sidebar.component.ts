import { Component, Input, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { RippleModule } from 'primeng/ripple';

interface ChildItem {
  label: string;
  icon: string;
  route: string;
}

interface DirectItem {
  type: 'direct';
  key: string;
  label: string;
  icon: string;
  route: string;
}

interface GroupItem {
  type: 'group';
  key: string;
  label: string;
  icon: string;
  children: ChildItem[];
}

type SidebarItem = DirectItem | GroupItem;

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [CommonModule, RouterLink, RouterLinkActive, RippleModule],
  templateUrl: './sidebar.component.html'
})
export class SidebarComponent implements OnInit {
  @Input() collapsed = false;

  readonly menu: SidebarItem[] = [
    { type: 'direct', key: 'dashboard', label: 'Dashboard', icon: 'pi pi-home', route: '/dashboard' },
    {
      type: 'group',
      key: 'system',
      label: 'Hệ thống',
      icon: 'pi pi-cog',
      children: [{ label: 'Người dùng', icon: 'pi pi-users', route: '/system/users' }]
    }
  ];

  openGroups = new Set<string>(['system']);

  constructor(private routerRef: Router) {}

  ngOnInit(): void {
    this.autoOpenActiveGroup();
  }

  toggleGroup(key: string): void {
    if (this.collapsed) return;
    if (this.openGroups.has(key)) {
      this.openGroups.delete(key);
    } else {
      this.openGroups.add(key);
    }
  }

  isGroupOpen(key: string): boolean {
    return this.openGroups.has(key);
  }

  private autoOpenActiveGroup(): void {
    for (const item of this.menu) {
      if (item.type === 'group' && item.children.some((c) => this.routerRef.isActive(c.route, { paths: 'subset', queryParams: 'ignored', fragment: 'ignored', matrixParams: 'ignored' }))) {
        this.openGroups.add(item.key);
      }
    }
  }
}
