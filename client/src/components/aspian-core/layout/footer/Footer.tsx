import React, { FC } from 'react';
import { Layout } from 'antd';
import { HeartFilled } from '@ant-design/icons';
import { withTranslation, WithTranslation, Trans } from 'react-i18next';
import {e2p} from '../../../../js/aspian-core/base/utilities';

const { Footer } = Layout;
const AspianFooter: FC<WithTranslation> = ({t}) => {
  
  return (
    <Footer style={{ textAlign: 'center', fontSize: '.65rem' }}>
      <div>
        <Trans>{t('footer.made-with')}</Trans><HeartFilled style={{ color: 'red' }} /><Trans>{t('footer.by')}</Trans>
        <a href="#!"><Trans>{t('footer.my-name')}</Trans></a>
      </div>
      <div>
      <Trans>{t('appName')}</Trans> - <Trans>{t('footer.copyright')}</Trans> &copy; {e2p(new Date().getFullYear().toString())}
      </div>
    </Footer>
  );
};

export default withTranslation()(AspianFooter);
