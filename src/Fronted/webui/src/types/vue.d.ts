import { Store } from 'vuex';

declare module '@vue/runtime-core' {
  interface State {
    number: number;
    isPublic: boolean;
  }

  interface ComponentCustomProperties {
    $store: Store<State>;
  }
}
