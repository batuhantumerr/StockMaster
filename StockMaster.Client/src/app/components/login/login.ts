import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { RouterModule, Router } from '@angular/router'; // Router eklendi
import { AuthService } from '../../services/auth'; // AuthService eklendi
import { ToastrService } from 'ngx-toastr'; // Toastr eklendi

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './login.html',
  styleUrl: './login.scss'
})
export class LoginComponent {
  loginForm: FormGroup;
  isLoading = false; // Butona basılınca loading dönmesi için

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private toastr: ToastrService
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(3)]]
    });
  }

  onSubmit() {
    if (this.loginForm.valid) {
      this.isLoading = true; // Yükleniyor...

      this.authService.login(this.loginForm.value).subscribe({
        next: (res) => {
          this.isLoading = false;
          this.toastr.success('Giriş Başarılı!', 'Hoşgeldiniz');
          this.router.navigate(['/dashboard']); // Birazdan yapacağız
        },
        error: (err) => {
          this.isLoading = false;
          // Backend'den gelen hatayı göster, yoksa genel hata ver
          const message = err.error?.errors?.[0] || 'Giriş başarısız. Bilgileri kontrol edin.';
          this.toastr.error(message, 'Hata');
        }
      });
    }
  }
}