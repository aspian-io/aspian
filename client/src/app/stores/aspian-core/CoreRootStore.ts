import {createContext} from 'react';
import PostStore from './post/postStore';
import LocaleStore from './locale/localeStore';
import SiderStore from './layout/siderStore';
import ResultStore from './layout/resultStore/resultStore';

export class CoreRootStore {
    postStore: PostStore;
    localeStore: LocaleStore;
    siderStore: SiderStore;
    resultStore: ResultStore;

    constructor() {
        this.postStore = new PostStore(this);
        this.localeStore = new LocaleStore(this);
        this.siderStore = new SiderStore(this);
        this.resultStore = new ResultStore(this);
    }
}

export const CoreRootStoreContext = createContext(new CoreRootStore());