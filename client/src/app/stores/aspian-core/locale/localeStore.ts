import { observable, action, configure, runInAction } from 'mobx';
import { CoreRootStore } from '../CoreRootStore';
import {
  LocaleVariableEnum,
  LanguageActionTypeEnum,
  DirectionActionTypeEnum,
} from './types';
import ClassicEditor from '@ckeditor/ckeditor5-build-classic';

configure({ enforceActions: 'observed' });

export default class LocaleStore {
  coreRootStore: CoreRootStore;
  constructor(coreRootStore: CoreRootStore) {
    this.coreRootStore = coreRootStore;
  }

  // Convert local storage string value to an anum key for language
  localStorageInitialLang =
    localStorage.getItem(LocaleVariableEnum.ASPIAN_CMS_LANG) !== null
      ? (localStorage.getItem(
          LocaleVariableEnum.ASPIAN_CMS_LANG
        ) as keyof typeof LanguageActionTypeEnum)
      : LanguageActionTypeEnum.en;
  // Convert local storage string value to an anum key for direction
  localStorageInitialDir =
    localStorage.getItem(LocaleVariableEnum.ASPIAN_CMS_DIR) !== null
      ? (localStorage.getItem(
          LocaleVariableEnum.ASPIAN_CMS_DIR
        ) as keyof typeof DirectionActionTypeEnum)
      : DirectionActionTypeEnum.LTR;

  // Convert local storage string value to enum for language and saving it in initialLang const for further usage
  initialLang = LanguageActionTypeEnum[this.localStorageInitialLang];
  // Convert local storage string value to enum for direction and saving it in initialDir const for further usage
  initialDir = DirectionActionTypeEnum[this.localStorageInitialDir];

  @observable lang: LanguageActionTypeEnum = this.initialLang;
  @observable dir: DirectionActionTypeEnum = this.initialDir;
  @observable ckEditorInstance: any = null;

  @action handleChangeLanguage = (
    lang: LanguageActionTypeEnum = LanguageActionTypeEnum.en
  ) => {
    localStorage.setItem(LocaleVariableEnum.ASPIAN_CMS_LANG, lang);
    const ckEditorInstance = this.ckEditorInstance;
    if (ckEditorInstance !== null) {
      // ckEditorInstance.destroy().then(() => {
      //   ClassicEditor.replace('editor', {language: this.lang})
      // });
      ckEditorInstance.destroy().then(() => {
        ClassicEditor
          .create( document.querySelector( '#addPostCkEditor' ), {
            language: this.lang
          } )
          .then( (editor: any) => {
            editor.model.document.on( 'change:data', () => {
              const data = editor.getData();
              console.log( data );
          } );
            runInAction('change language and dir to LTR', () => {
              this.ckEditorInstance = editor;
            });
          } )
      })
      
    }

    // For English language
    if (lang === LanguageActionTypeEnum.en) {
      localStorage.setItem(
        LocaleVariableEnum.ASPIAN_CMS_DIR,
        DirectionActionTypeEnum.LTR
      );

      runInAction('change language and dir to LTR', () => {
        this.lang = lang;
        this.dir = DirectionActionTypeEnum.LTR;
      });
    }

    // For Persian (Farsi) language
    if (lang === LanguageActionTypeEnum.fa) {
      localStorage.setItem(
        LocaleVariableEnum.ASPIAN_CMS_DIR,
        DirectionActionTypeEnum.RTL
      );

      runInAction('change language and dir to RTL', () => {
        this.lang = lang;
        this.dir = DirectionActionTypeEnum.RTL;
      });
    }
  };

  @action storeCkEditorInstance = (editor: any) => {
    this.ckEditorInstance = editor;
  }
}
