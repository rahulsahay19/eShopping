import { HttpHeaders } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { AcntService } from '../account/acnt.service';
import { BasketService } from '../basket/basket.service';
import { IBasket, IBasketItem } from '../shared/models/basket';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.scss']
})
export class CheckoutComponent implements OnInit {
  constructor(public basketService: BasketService, private acntService: AcntService){}

  ngOnInit(): void {
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


  removeBasketItem(item: IBasketItem){
    this.basketService.removeItemFromBasket(item);
  }

  incrementItem(item: IBasketItem){
    this.basketService.incrementItemQuantity(item);
  }

  decrementItem(item: IBasketItem){
    this.basketService.decrementItemQuantity(item);
  }

  orderNow(item: IBasket){
    this.basketService.checkoutBasket(item);
  }
}
