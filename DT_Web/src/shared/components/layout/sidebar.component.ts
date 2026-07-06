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
      key: 'category',
      label: 'Danh mục',
      icon: 'pi pi-folder',
      children: [{ label: 'Chức vụ', icon: 'pi pi-briefcase', route: '/danh-muc/chuc-vu' },
      { label: 'Tin tức', icon: 'pi pi-book', route: '/danh-muc/tin-tuc' },
      { label: 'Danh mục sản phẩm', icon: 'pi pi-box', route: '/danh-muc/san-pham' }
      ]
    },
    {
      type: 'group',
      key: 'system',
      label: 'Hệ thống',
      icon: 'pi pi-cog',
      children: [{ label: 'Người dùng', icon: 'pi pi-users', route: '/system/users' }]
    }
  ];

  openGroups = new Set<string>(['system']);

  hoveredGroup: string | null = null;
  flyoutTop = 0;
  flyoutLeft = 0;

  hoveredDirect: string | null = null;
  tooltipTop = 0;
  tooltipLeft = 0;

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

  isGroupActive(item: GroupItem): boolean {
    return item.children.some((c) =>
      this.routerRef.isActive(c.route, { paths: 'subset', queryParams: 'ignored', fragment: 'ignored', matrixParams: 'ignored' })
    );
  }

  onGroupMouseEnter(key: string, event: MouseEvent): void {
    const rect = (event.currentTarget as HTMLElement).getBoundingClientRect();
    this.flyoutTop = rect.top;
    this.flyoutLeft = rect.right + 8;
    this.hoveredGroup = key;
  }

  onGroupMouseLeave(): void {
    this.hoveredGroup = null;
  }

  isFlyoutOpen(key: string): boolean {
    if (this.hoveredGroup !== key) return false;
    return this.collapsed || !this.isGroupOpen(key);
  }

  onDirectMouseEnter(key: string, event: MouseEvent): void {
    if (!this.collapsed) return;
    const rect = (event.currentTarget as HTMLElement).getBoundingClientRect();
    this.tooltipTop = rect.top;
    this.tooltipLeft = rect.right + 8;
    this.hoveredDirect = key;
  }

  onDirectMouseLeave(): void {
    this.hoveredDirect = null;
  }

  isTooltipOpen(key: string): boolean {
    return this.collapsed && this.hoveredDirect === key;
  }

  private autoOpenActiveGroup(): void {
    for (const item of this.menu) {
      if (item.type === 'group' && this.isGroupActive(item)) {
        this.openGroups.add(item.key);
      }
    }
  }
}