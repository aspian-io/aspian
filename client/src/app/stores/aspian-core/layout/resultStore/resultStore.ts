import { CoreRootStore } from '../../CoreRootStore';
import { observable, configure, action } from 'mobx';
import { ResultStatusEnum } from './types';

configure({ enforceActions: 'observed' });

export default class ResultStore {
  coreRootStore: CoreRootStore;
  constructor(coreRootStore: CoreRootStore) {
    this.coreRootStore = coreRootStore;
  }

  @observable status: ResultStatusEnum = ResultStatusEnum.Info;
  @observable title: string = '';
  @observable subTitle: string = '';
  @observable primaryBtnText: string = '';
  @observable primaryBtnLink: string = '';
  @observable ghostBtnText: string = '';
  @observable ghostBtnLink: string = '';
  @observable errorMsgList: string[] = [];

  @action setResultPage = (
    status: ResultStatusEnum = ResultStatusEnum.Info,
    title: string = '',
    subTitle: string = '',
    primaryBtnText: string = '',
    primaryBtnLink: string = '',
    ghostBtnText: string = '',
    ghostBtnLink: string = '',
    errorMsgList: string[] = []
  ) => {
    this.status = status;
    this.title = title;
    this.subTitle = subTitle;
    this.primaryBtnText = primaryBtnText;
    this.primaryBtnLink = primaryBtnLink;
    this.ghostBtnText = ghostBtnText;
    this.ghostBtnLink = ghostBtnLink;
    this.errorMsgList = errorMsgList;
  };
}
