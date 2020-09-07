import React, { useContext } from 'react';
import { Result, Button, Typography } from 'antd';
import { CloseCircleOutlined } from '@ant-design/icons';
import { CoreRootStoreContext } from '../../../../app/stores/aspian-core/CoreRootStore';
import { history } from '../../../..';
import { v4 as uuidv4 } from 'uuid';
import { useTranslation } from 'react-i18next';

const { Paragraph, Text } = Typography;

const ResultPage = () => {
  const { t } = useTranslation('core_resultPage');
  /// Stores
  const coreRootStore = useContext(CoreRootStoreContext);
  const {
    status,
    title,
    subTitle,
    primaryBtnText,
    primaryBtnLink,
    ghostBtnText,
    ghostBtnLink,
    errorMsgList,
  } = coreRootStore.resultStore;

  return (
    <Result
      status={status}
      title={title}
      subTitle={subTitle}
      extra={[
        <Button
          type="primary"
          key={uuidv4()}
          style={
            primaryBtnText ? { display: 'inline-block' } : { display: 'none' }
          }
          onClick={() => history.push(primaryBtnLink)}
        >
          {primaryBtnText}
        </Button>,
        <Button
          key={uuidv4()}
          style={
            ghostBtnText ? { display: 'inline-block' } : { display: 'none' }
          }
          onClick={() => history.push(ghostBtnLink)}
        >
          {ghostBtnText}
        </Button>,
      ]}
    >
      {errorMsgList.length > 0 && (
        <div className="desc">
          <Paragraph key={uuidv4()}>
            <Text
              strong
              style={{
                fontSize: 16,
              }}
            >
              {t('description')}
            </Text>
          </Paragraph>
          {errorMsgList.map((msg, i) => {
            return (
              <Paragraph key={uuidv4()}>
                <CloseCircleOutlined style={{ color: 'red' }} /> {msg}
              </Paragraph>
            );
          })}
        </div>
      )}
    </Result>
  );
};

export default ResultPage;
