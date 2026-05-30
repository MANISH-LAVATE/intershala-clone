export interface NotificationItem {
  id: number;
  type: string;
  title: string;
  message: string;
  actionUrl: string | null;
  isRead: boolean;
  readAt: string | null;
  createdAt: string;
}

export interface NotificationSummary {
  items: NotificationItem[];
  unreadCount: number;
}
