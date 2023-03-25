import { Component, OnInit } from '@angular/core';
import { AcntService } from 'src/app/account/acnt.service';
import { BasketService } from 'src/app/basket/basket.service';
import { IBasketItem } from 'src/app/shared/models/basket';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
  constructor(public basketService: BasketService, private acntService: AcntService){}
  ngOnInit(): void {

    console.log(`current user:`);
    this.acntService.currentUser$.subscribe({
      next:(res) =>{
        this.isUserAuthenticated = res;
        console.log(this.isUserAuthenticated);
      },error:(err) =>{
        console.log(`An error occurred while setting isUserAuthenticated flag.`)
      }
    })
  }
  public isUserAuthenticated: boolean = false;
  getBasketCount(items: IBasketItem[]){
    return items.reduce((sum, item)=>sum + item.quantity, 0);
  }
  public login = () => {
    this.acntService.login();
  }
  public logout = () => {
    this.acntService.signout();
  }
}
