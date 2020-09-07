import React from 'react';
import { Result, Button } from 'antd';
import { history } from '../../../..';
import { useTranslation } from 'react-i18next';

function Error() {
  const { t } = useTranslation('core_error');

  return (
    <Result
      status="error"
      title={t('title')}
      subTitle={t('subTitle')}
      extra={
        <Button type="primary" onClick={() => history.push('/admin')}>
          {t('btn')}
        </Button>
      }
    />
  );
}

export default Error;
