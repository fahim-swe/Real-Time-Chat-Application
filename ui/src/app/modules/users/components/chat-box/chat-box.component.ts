import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { message } from '@common/models/message';
import { sendmessage } from '@common/models/sendmessage';
import { MessageService } from '@common/services/message/message.service';
import { sendMessage } from '@microsoft/signalr/dist/esm/Utils';
import { distinctUntilChanged, take } from 'rxjs';


@Component({
  selector: 'app-chat-box',
  templateUrl: './chat-box.component.html',
  styleUrls: ['./chat-box.component.scss']
})
export class ChatBoxComponent implements OnInit, OnDestroy {

  recieverId : string;
  recieverUsername : string;
  message: message[];
  sendmessage : sendmessage;
  sendNewSMS : FormGroup;

 

  scrolltop:number;
  @ViewChild('commentEl') comment : ElementRef ;  

  Ini()
  {
    this.sendNewSMS = this.fb.group({
      content: [''],
      recipientId : [this.recieverId],
    })
  }
  

  constructor(private fb: FormBuilder,
    private router: Router,
    private activeRoute: ActivatedRoute, public messageService: MessageService
   ) {}

  ngOnDestroy(): void {
    console.log("Stop HubConnection");
    this.messageService.stopHubConnection();
  }


  ngOnInit(): void {
    const id = (this.activeRoute.snapshot.paramMap.get('id'));
    if(id != null){
      this.recieverId = id;
    }

    const username = (this.activeRoute.snapshot.paramMap.get("username"));
    if(username != null){
      this.recieverUsername = username;
    }
    this.Ini();
    this.messageService.createHubConnection(this.recieverId);

    this.messageService.messageThread$.pipe(distinctUntilChanged())
      .subscribe(res =>{
        this.scrolltop = this.comment.nativeElement.scrollHeight;
      });
  }

  
  getUserMessage(){
    this.messageService.messageThread$.subscribe(res => {
     
      this.message = res as unknown as message[];
      console.log(this.message);
    })
  }

  sendSMS()
  {
     
     this.messageService.sendMessage(this.sendNewSMS.value).then(
      ()=>{
        this.sendNewSMS.reset();
        this.Ini();
        this.scrolltop = this.comment.nativeElement.scrollHeight;
      }
     )
  }
  
}
