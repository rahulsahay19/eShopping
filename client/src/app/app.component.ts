import { Component, OnInit } from '@angular/core';
import { AcntService } from './account/acnt.service';
import { BasketService } from './basket/basket.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'eShopping';

  constructor(private basketService: BasketService, private acntService: AcntService){}
  ngOnInit(): void {

    const basket_username = localStorage.getItem('basket_username');
    if(basket_username){
      this.basketService.getBasket(basket_username);
    }
  }

}
