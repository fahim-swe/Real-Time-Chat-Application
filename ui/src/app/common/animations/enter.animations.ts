import { animate, style, transition, trigger } from '@angular/animations';
import { Directions } from '@common/enums/directions.enum';
import { SlideInAnimationMetadata } from '../models/animation-metadata.model';

export const fadeIn = trigger('fadeIn', [
  transition(':enter', [style({ opacity: 0 }), animate(800)]),
]);

function getTranformValue(direction: Directions, position: string): string {
  let value: string;
  switch (direction) {
    case Directions.TOP:
      value = 'translateY(-' + position + ')';
      break;
    case Directions.BOTTOP:
      value = 'translateY(' + position + ')';
      break;
    case Directions.RIGHT:
      value = 'translateX(' + position + ')';
      break;
    case Directions.LEFT:
      value = 'translateX(-' + position + ')';
      break;
    default:
      value = 'translateX(' + position + ')';
  }
  return value;
}

function getTriggerValue(direction: Directions): string {
  let value: string;
  switch (direction) {
    case Directions.TOP:
      value = 'slideInFromT';
      break;
    case Directions.BOTTOP:
      value = 'slideInFromD';
      break;
    case Directions.RIGHT:
      value = 'slideInFromR';
      break;
    case Directions.LEFT:
      value = 'slideInFromL';
      break;
    default:
      value = 'slideInFromR';
  }
  return value;
}
export const slideIn = (value: SlideInAnimationMetadata) => {
  if (!value.position) value.position = '200px';
  if (!value.duration) value.duration = '800ms';
  if (!value.delay) value.delay = '0ms';
  if (!value.direction) value.direction = Directions.RIGHT;

  return trigger(getTriggerValue(value.direction), [
    transition(':enter', [
      style({ transform: getTranformValue(value.direction, value.position) }),
      animate(value.duration + ' ' + value.delay),
    ]),
  ]);
};
