import React from 'react';
import { Result, Button } from 'antd';
import { useTranslation } from 'react-i18next';

const ServerError = () => {
  const { t } = useTranslation('core_serverErrorPage');
  return (
    <Result
      status="500"
      title={t('title')}
      subTitle={t('subtitle')}
      extra={
        <Button type="primary" href="/admin">
          {t('button')}
        </Button>
      }
    />
  );
};

export default ServerError;
