import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, of } from 'rxjs';
import { HubConnection, HubConnectionBuilder, LogLevel } from '@microsoft/signalr';
import { environment } from '../../../environments/environment';
import { API_ENDPOINTS } from '../constants/api-endpoints.constant';
import { ApiResponse } from '../../shared/models/api-response.model';
import { NotificationItem, NotificationSummary } from './notification.model';
import { TokenStorageService } from '../auth/services/token-storage.service';

@Injectable({ providedIn: 'root' })
export class NotificationService {
  private readonly http = inject(HttpClient);
  private readonly tokenStorage = inject(TokenStorageService);
  private readonly base = environment.apiBaseUrl;
  private hubConnection: HubConnection | null = null;

  readonly unreadCount = signal(0);
  readonly notifications = signal<NotificationItem[]>([]);

  getNotifications(page = 1, pageSize = 20): Observable<ApiResponse<NotificationSummary>> {
    return this.http.get<ApiResponse<NotificationSummary>>(
      `${this.base}${API_ENDPOINTS.NOTIFICATIONS.BASE}`,
      { params: { page, pageSize } },
    );
  }

  markRead(id: number): Observable<void> {
    return this.http.post<void>(
      `${this.base}${API_ENDPOINTS.NOTIFICATIONS.MARK_READ(id)}`,
      {},
    );
  }

  markAllRead(): Observable<void> {
    return this.http.post<void>(
      `${this.base}${API_ENDPOINTS.NOTIFICATIONS.MARK_ALL_READ}`,
      {},
    );
  }

  loadNotifications(): void {
    this.getNotifications()
      .pipe(catchError(() => of(null)))
      .subscribe((res) => {
        if (res?.data) {
          this.notifications.set(res.data.items);
          this.unreadCount.set(res.data.unreadCount);
        }
      });
  }

  startSignalRConnection(token: string): void {
    if (this.hubConnection) return;

    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${environment.signalRHubUrl}/notifications`, {
        accessTokenFactory: () => token,
      })
      .withAutomaticReconnect()
      .configureLogging(environment.production ? LogLevel.Error : LogLevel.Warning)
      .build();

    this.hubConnection.on('ReceiveNotification', (notification: NotificationItem) => {
      this.notifications.update((n) => [notification, ...n]);
      this.unreadCount.update((c) => c + 1);
    });

    this.hubConnection.on('UnreadCount', (count: number) => {
      this.unreadCount.set(count);
    });

    this.hubConnection.start().catch(() => {
      // Hub unavailable in dev without backend — silently ignore
    });
  }

  stopSignalRConnection(): void {
    this.hubConnection?.stop();
    this.hubConnection = null;
  }
}
