import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { LoginDto } from '../models/login.dto';
import { TokenDto } from '../models/token.dto';
import { CustomResponse } from '../models/response.dto';
import { environment } from '../../environments/environment';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient) { }

  login(loginDto: LoginDto) {
    // API'ye POST isteği atıyoruz
    return this.http.post<CustomResponse<TokenDto>>(`${environment.baseUrl}/auth/login`, loginDto)
      .pipe(
        map(response => {
          // Eğer cevap başarılıysa ve data varsa token'ı kaydet
          if (response.data && response.data.accessToken) {
            localStorage.setItem('token', response.data.accessToken);
          }
          return response;
        })
      );
  }

  // Kullanıcı çıkış yaparsa token'ı sil
  logout() {
    localStorage.removeItem('token');
  }

  // Token var mı diye kontrol et (Basit kontrol)
  isAuthenticated(): boolean {
    return !!localStorage.getItem('token');
  }
}